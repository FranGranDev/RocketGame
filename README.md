# Rocket Game

## Технологии
- Unity 2021.3.20f1
- Zenject
- DOTween

## Система бесконечного уровня
Уровень строиться процедурно из отдельных преград, на основе значения *Perlin Noise* на каждой нормализованной по размеру преграды высоте. При одинаковом `seed` будет всегда генерироваться тот же уровень. Для оптимизации используется пул объектов преград. В начале игры создается `X` преград, которые переиспользуются на всем протяжении игры.

Игра продолжается бесконечно, пока ракета не упадет за пределы видимости камеры.

### Преграды
Уровень строиться из [Pipe.cs](Assets/Scripts/Enviroment/Pipe.cs), которая представляет из себя преграду слева, преграду справа и задник.
Для создания [Pipe.cs](Assets/Scripts/Enviroment/Pipe.cs) используется метод `Show`, который принимает высоту создания, сложность и мультипликатор для левой и правой преграды.
```csharp
public void Show(float height, float difficult, float xRatio)
{
    this.xRatio = xRatio;
    this.difficult = difficult;

    transform.position = Vector3.up * height;

    SetObstacles();

    gameObject.SetActive(true);
}

private void SetObstacles()
{
    float leftX = Mathf.Lerp(0, maxOffset, offsetCurve.Evaluate(difficult * xRatio));
    float rightX = Mathf.Lerp(0, maxOffset, offsetCurve.Evaluate(difficult * (1 - xRatio)));

    leftObstacle.localPosition = new Vector3(leftX, 0, 0);
    rightObstacle.localPosition = new Vector3(-rightX, 0, 0);
}
```

Преграды создает [PipeFactory](Assets/Scripts/Enviroment/PipeFactory.cs)
```csharp
public class PipeFactory : IFactory<Pipe>
{
    [Inject]
    private PipeData pipeData;

    [Inject]
    private DiContainer container;


    public Pipe Create()
    {
        return container.InstantiatePrefabForComponent<Pipe>(pipeData.Pipe);
    }
}
```

### Создание бесконечного уровня
За создание бесконечного уровня ответственный [InfinityPipeCreator.cs](Assets/Scripts/Enviroment/InfinityPipeCreator.cs)

**Обновление преград**
```csharp
private void UpdatePipes()
{
    currantIndex = Mathf.RoundToInt(CurrantHeight / createOffset);

    if (currantIndex == prevIndex)
    {
        return;
    }
    prevIndex = currantIndex;

    CreatePipes();
    HidePipes();
}
private void CreatePipes()
{
    for (int i = -createOffsetIndex; i < createOffsetIndex; i++)
    {
        if (pipes.ContainsKey(currantIndex + i))
        {
            ShowPipe(currantIndex + i);
            continue;
        }

        CreatePipe(currantIndex + i);
    }
}
private void HidePipes()
{
    for (int i = -hideOffsetIndex; i < hideOffsetIndex; i++)
    {
        if (i > -createOffsetIndex && i < createOffsetIndex)
            continue;

        if (!pipes.ContainsKey(currantIndex + i))
            continue;

        HidePipe(currantIndex + i);
    }
}
```

**Пул преград**
```csharp
private class PipesPool
{
    public PipesPool(IFactory<Pipe> factory, Transform container, int count)
    {
        pipes = new List<Pipe>(count);

        for(int i = 0; i < count; i++)
        {
            Pipe pipe = factory.Create();
            pipe.Hide();
            pipe.transform.parent = container;

            pipes.Add(pipe);
        }
    }

    private List<Pipe> pipes { get; }       
    
    public Pipe Get()
    {
        try
        {
            return pipes.First(x => x.Hidden);
        }
        catch
        {
            return pipes.First(); // If move infinity fast, that happen :)
        }
    }
    public void Reset()
    {
        pipes.ForEach(x => x.Hide());
    }
}
```

**Создание и уничтожение преград**
```csharp
private void CreatePipe(int index)
{
    pipes.Add(index, pipesPool.Get());

    ShowPipe(index);
}
private void ShowPipe(int index)
{
    float normalizedHeight = NormalizedHeight(index);

    if (index % 3 == 0) //Для избежания невозможных преград
    {
        float xRatio = XRatioNormalized(xRatioNoise.Evaluate(index));
        float difficultResult = difficult.Get(normalizedHeight);

        pipes[index].Show(normalizedHeight, difficultResult, xRatio);
        return;
    }

    pipes[index].Show(normalizedHeight, 0, 0);
}

private void HidePipe(int index)
{
    pipes[index].Hide();

    pipes.Remove(index);
}
```


## Управление ракетой
[Rocket.cs](Assets/Scripts/Player/Rocket.cs) управляется со скрипта [RocketController.cs](Assets/Scripts/Player/RocketController.cs)

```csharp
[RequireComponent(typeof(IDrivable))]
public class RocketController : MonoBehaviour
{
    [SerializeField, Range(0.5f, 3f)] private float sensivity;
    [Inject]
    private IScreenInput screenInput;
    
    private IDrivable drivable;

    private void Awake()
    {
        drivable = GetComponent<IDrivable>();
    }
    private void OnEnable()
    {
        screenInput.OnDrag += OnDrag;
        screenInput.OnRelease += OnRelease;
        screenInput.OnTap += OnTap;
    }
    private void OnDisable()
    {
        screenInput.OnDrag -= OnDrag;
        screenInput.OnRelease -= OnRelease;
        screenInput.OnTap -= OnTap;
    }

    private void OnTap()
    {
        drivable.EngineOn();
    }
    private void OnRelease()
    {
        drivable.EngineOff();
    }
    private void OnDrag(Vector2 point)
    {
        drivable.Move(point * sensivity);
    }
}
```
[RocketController.cs](Assets/Scripts/Player/RocketController.cs) имеет зависимость от интерфейса`IScreenInput`, имеющий единственную реализацию [ScreenInput.cs](Assets/Scripts/Input/ScreenInput.cs) (лучше конечно делать две реализации, для Unity и для телефонов, и выбирать нужный компонент в MonoInstaller)

```csharp
public class ScreenInput : MonoBehaviour, IScreenInput
{
    [Header("Settings")]
    [SerializeField] private LayerMask mask;


    private bool touched;
    private Vector2 lastPoint;


    public event Action OnTap;
    public event Action OnRelease;
    public event Action<Vector2> OnDrag;


    private void MouseInput()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && !touched)
        {
            Tap();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && touched)
        {
            Release();
        }
        if(Input.GetKey(KeyCode.Mouse0))
        {
            Move(Input.mousePosition);
        }
    }
    private void TapInput()
    {
        if (Input.touchCount > 0 && !touched)
        {
            Tap();
        }
        if (Input.touchCount == 0 && touched)
        {
            Release();
        }
        if (touched)
        {
            Move(Input.GetTouch(0).position);
        }
    }

    private void Tap()
    {
        touched = true;

        OnTap?.Invoke();
    }
    private void Release()
    {
        touched = false;
        lastPoint = default;

        OnRelease?.Invoke();
    }
    private void Move(Vector2 screenPoint)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);

        if(Physics.Raycast(ray, out RaycastHit hit, 1000, mask))
        {
            Vector2 point = hit.point;
            Vector2 delta = (point - lastPoint) / Time.deltaTime;

            if (lastPoint == default)
            {
                delta = Vector2.zero;
            }
            lastPoint = point;

            OnDrag?.Invoke(delta);
        }
    }


    private void Update()
    {
#if UNITY_EDITOR
        MouseInput();
#else
        TapInput();
#endif
    }
    private void Start()
    {
        Vector3 position = transform.position;
        position.z = 0;

        transform.position = position;
    }
}
```

# Полет ракеты
[Rocket.cs](Assets/Scripts/Player/Rocket.cs) летит исключительно по физике, без использования кинематики

**Вызовы методов для полета и анимаций**
```csharp
private void FixedUpdate()
{
    if(IsDriving)
    {
        Drive();
        Animate();
    }
    else
    {
        Gravity();
        XDrag();
    }

    Effects();
}
```

**Полет ракеты**
```csharp
private void Drive()
{
    float currantAcceleration = acceleration * (rigidbody.velocity.y > 0 ? 1 : 0.5f);

    float velocityY = Mathf.Sqrt(Mathf.Max(maxSpeed * maxSpeed - rigidbody.velocity.x * rigidbody.velocity.x * xMoveSpeedRatio, 0));
    Vector3 targetVelocity = new Vector3(rigidbody.velocity.x, velocityY);
    targetVelocity = Vector3.Lerp(targetVelocity.normalized, transform.up, directionRatio) * targetVelocity.magnitude;

    rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, targetVelocity, currantAcceleration);

    float velocityXRatio = Mathf.Abs(rigidbody.velocity.y / maxSpeed);
    rigidbody.AddForce(Vector3.right * moveDirection * moveSpeed * velocityXRatio, ForceMode.Acceleration);
    rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, Vector3.up * rigidbody.velocity.y, moveSmooth);


    float velocityX = rigidbody.velocity.x;
    rigidbody.velocity = new Vector3(Mathf.Clamp(velocityX, -moveSpeed, moveSpeed), rigidbody.velocity.y, 0);
}
```
**Включение/выключение двигателя**
```csharp
public void EngineOn()
{
    rigidbody.angularVelocity = Vector3.zero;

    isDriving = true;
}
public void EngineOff()
{
    float xPower = rigidbody.velocity.x * speedXfallAngularRatio + transform.up.x * directionXfallAngular;
    Vector3 torque = new Vector3(0, 0, -xPower);
    rigidbody.AddTorque(torque, ForceMode.VelocityChange);

    isDriving = false;
}
```

**Анимация направления движения**
```csharp
private void Animate()
{
    Vector3 direction = new Vector3(rigidbody.velocity.x * turnPower, Mathf.Max(Mathf.Abs(rigidbody.velocity.y) * 0.75f, 1), 0).normalized;
    Quaternion rotation = Quaternion.LookRotation(transform.forward, direction);
    rotation.eulerAngles = new Vector3(0, 0, rotation.eulerAngles.z);

    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, turnSmooth);
    rigidbody.angularVelocity = Vector3.Lerp(rigidbody.angularVelocity, Vector3.zero, 0.1f);
}
```

**Симуляция гравитации и сопративление**
```csharp
private void Gravity()
{
    rigidbody.AddForce(Vector3.up * gravity, ForceMode.Acceleration);
}

private void XDrag()
{
    Vector3 velocity = rigidbody.velocity;
    velocity.x *= (1 - xDrag);

    rigidbody.velocity = velocity;
}
```
