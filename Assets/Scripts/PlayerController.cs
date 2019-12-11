using System.Security.Cryptography;
using InputConfiguration;
using Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Engine))]
[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform shipTransform;
    [SerializeField] private Transform cameraAnchor;
    
    private Vector3 _screenSize;
    private Vector3 _defaultLocalCameraAnchorPosition;
    public bool blockRotationInRearView = false;
 
    public float shipAlignSpeed = 2;

    public bool invertX = false;
    public bool invertY = false;

    public int controlCircleRadius = 200;
    
    private float _remainingBoost;
    private bool _isOverheated;
    public float GetOverheatRatio() => _remainingBoost / _engine.boostDuration;

    public float strafeAnimationSpeed = 2;
    public float boostShakeStrength = 1.5f;
    
    private bool _strafeLeft;
    private bool _strafeLeftDown;
    private bool _strafeRight;
    private bool _strafeRightDown;
    private bool _boost;

    [Range(1, 3)] public int rollRotationCount = 1;
    [Range(1, 5)] public float rollCooldown = 3;
    public float rollTimeFrame = 1f;
    public bool isRolling = false;
    public int rollDirection = 0;
    public float rollProgress = 0;

    private float _lastRollTime;
    private float _strafeLeftDownTime;
    private float _strafeRightDownTime;

    private Engine _engine;
    
    [SerializeField] private TextMeshProUGUI uiTextSpeed;
    [SerializeField] private Image uiOverheatGauge;
    
    [SerializeField] private SkillId[] skills = new SkillId[10];
    [SerializeField] private float[] skillTimers = new float[10];
    
    void Start()
    {
        _screenSize = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        _defaultLocalCameraAnchorPosition = cameraAnchor.transform.localPosition;

        _engine = GetComponent<Engine>();
        _remainingBoost = _engine.boostDuration;
    }
    
    void Update()
    {
        ProcessInput();
        AdjustStrafeValue();
        ProcessRollInput();
        AdjustRotation();
        AdjustSpeed();
        AdjustOverheat();
        ShakeShip();

        uiTextSpeed.text = (_engine.currentSpeed * 10).ToString("F0");
        uiOverheatGauge.fillAmount = GetOverheatRatio();

        // vvvv Skill test vvvv
        // TODO: Check all Skill Keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var skillId = skills[1]; // <- alpha 1 => array entry 1
            if (skillId == SkillId.None)
            {
                return;
            }

            var skill = SkillDictionary.GetSkill(skillId);
            skill.Execute(this);
        }
    }

    private void AdjustOverheat()
    {
        if (_boost && !isRolling && !_isOverheated)
        {
            _remainingBoost -= Time.deltaTime;
        }
        else
        {
            _remainingBoost += Time.deltaTime * 2;
        }

        if (_remainingBoost < 0)
        {
            _isOverheated = true;
        }

        if (_remainingBoost >= _engine.boostDuration)
        {
            _remainingBoost = _engine.boostDuration;
            _isOverheated = false;
        }
    }

    private void ShakeShip()
    {
        if (_boost && !_isOverheated)
        {
            var x = (Random.value * 2.0f - 1.0f) * boostShakeStrength;
            var y = (Random.value * 2.0f - 1.0f) * boostShakeStrength;
            var z = (Random.value * 2.0f - 1.0f) * boostShakeStrength;

            shipTransform.localPosition = Vector3.Lerp(shipTransform.localPosition, new Vector3(x, y, z), Time.deltaTime);
        }
        else
        {
            shipTransform.localPosition = Vector3.Lerp(shipTransform.localPosition, Vector3.zero, Time.deltaTime);
        }
    }

    private void ProcessInput()
    {
        _strafeLeft = KeyBindings.IsStrafeLeft();
        _strafeRight = KeyBindings.IsStrafeRight();

        _strafeLeftDown = KeyBindings.IsStrafeLeftDown();
        _strafeRightDown = KeyBindings.IsStrafeRightDown();

        _boost = KeyBindings.IsBoost();;
    }

    private void ProcessRollInput()
    {
        if (isRolling)
        {
            return;
        }
        
        if (_strafeLeftDown)
        {
            if (Time.time - _strafeLeftDownTime < rollTimeFrame)
            {
                if (Time.time - _lastRollTime > rollCooldown)
                {
                    StartRolling(1);
                }
            }
            
            _strafeLeftDownTime = Time.time;
        }

        if (_strafeRightDown)
        {
            if (Time.time - _strafeRightDownTime < rollTimeFrame)
            {
                if (Time.time - _lastRollTime > rollCooldown)
                {
                    StartRolling(-1);
                }
            }
            
            _strafeRightDownTime = Time.time;
        }
    }

    private void StartRolling(int direction)
    {
        rollDirection = direction;
        rollProgress = 0;
        isRolling = true;
        _lastRollTime = Time.time;
    }

    private void AdjustRotation()
    {
        if (isRolling)
        {
            shipTransform.Rotate(0, 0, Time.deltaTime * 360 * rollRotationCount * rollDirection);
            rollProgress += Time.deltaTime;
            if (rollProgress > 1)
            {
                isRolling = false;
            }
            return;
        }

        Vector2 clampedMovementPercentage;
        if (blockRotationInRearView && KeyBindings.IsRearCamera())
        {
            clampedMovementPercentage = Vector2.zero;
        }
        else
        {
            var cursorPos = Input.mousePosition;
            var relativePos = cursorPos - _screenSize;
            var clampedPos = Vector2.ClampMagnitude(relativePos, controlCircleRadius);
            clampedMovementPercentage = Vector2.ClampMagnitude(clampedPos / controlCircleRadius, 1);
        }
        
        _engine.requestedRotationAmount = new Vector3(
            clampedMovementPercentage.y * (invertX ? 1 : -1),
            clampedMovementPercentage.x * (invertY ? -1 : 1),
            0);
        
        var strafeRotation = Mathf.Lerp(-120, 120, (_engine.strafeValue + 1) * 0.5f);

        var targetShipRotation = new Vector3(
            clampedMovementPercentage.y * 20 * (invertX ? 1 : -1),
            0,
            Mathf.Clamp(clampedMovementPercentage.x * 60 * (invertY ? 1 : -1) + strafeRotation, -60, 60));

        shipTransform.localRotation = Quaternion.Lerp(
            shipTransform.localRotation, 
            Quaternion.Euler(targetShipRotation),
            Time.deltaTime * shipAlignSpeed);
    }

    private void AdjustStrafeValue()
    {
        if ((!_strafeLeft && !_strafeRight) || (_strafeLeft && _strafeRight))
        {
            if (_engine.strafeValue > 0)
            {
                _engine.strafeValue -= strafeAnimationSpeed * Time.deltaTime;
                if (_engine.strafeValue < 0)
                {
                    _engine.strafeValue = 0;
                }
            } 
            else
            {
                _engine.strafeValue += strafeAnimationSpeed * Time.deltaTime;
                if (_engine.strafeValue > 0)
                {
                    _engine.strafeValue = 0;
                }
            }
        }
        else
        {
            if (_strafeLeft)
            {
                _engine.strafeValue += strafeAnimationSpeed * Time.deltaTime;
            } 
            else
            {
                _engine.strafeValue -= strafeAnimationSpeed * Time.deltaTime;
            }

            _engine.strafeValue = Mathf.Clamp(_engine.strafeValue, -1f, 1f);
        }
    }

    private void AdjustSpeed()
    {
        if (isRolling)
        {
            _engine.currentSpeed -= _engine.deceleration * Time.deltaTime;
            if (_engine.currentSpeed < 0)
            {
                _engine.currentSpeed = 0;
            }
            
            return;
        }
        
        if (_boost && !_isOverheated)
        {
            if (_engine.currentSpeed < _engine.boostSpeed)
            {
                _engine.currentSpeed += _engine.acceleration * Time.deltaTime;
            }
            else
            {
                _engine.currentSpeed = _engine.boostSpeed;
            }

            return;
        }
    
        if (_engine.currentSpeed < _engine.minSpeed)
        {
            _engine.currentSpeed += _engine.acceleration * Time.deltaTime;
            return;
        }

        if (_engine.currentSpeed > _engine.maxSpeed)
        {
            _engine.currentSpeed -= _engine.deceleration * Time.deltaTime;
            return;
        }

        if (_engine.currentSpeed > _engine.minSpeed && KeyBindings.IsBrake())
        {
            _engine.currentSpeed -= _engine.deceleration * Time.deltaTime;
            return;
        }

        _engine.currentSpeed += _engine.acceleration * Time.deltaTime;
    }
}
