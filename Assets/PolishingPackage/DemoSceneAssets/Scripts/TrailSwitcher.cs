using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TrailSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _buttonGrid;
    private TrailRenderer[] _renderers;

    private const float SPEED = 10f;
    private Vector3 _moveDirection;

    private bool _goToTarget = false;
    private Vector3 _targetPos;
    private Vector3 _startingPos;
    private float _timeToTravel;
    private float _elapsed;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<TrailRenderer>();
        foreach (TrailRenderer trailRenderer in _renderers)
        {
            GameObject buttonObject = new GameObject(trailRenderer.name);
            buttonObject.transform.SetParent(_buttonGrid.transform);
            buttonObject.AddComponent<Image>();
            Button button = buttonObject.AddComponent<Button>();

            TrailRenderer render = trailRenderer;
            button.onClick.AddListener(() => OnSwitchTrail(render));

            GameObject textObject = new GameObject("Text");
            textObject.transform.SetParent(buttonObject.transform);
            TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
            text.color = Color.black;
            text.alignment = TextAlignmentOptions.Center;
            text.text = buttonObject.name;
            
            textObject.GetComponent<RectTransform>().sizeDelta = Vector2.one * 100;
        }
        DisableAll();
        OnSwitchTrail(_renderers[0]);
    }

    private void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _targetPos = new Vector3(hit.point.x, 0f, hit.point.z);
                _startingPos = transform.position;
                _timeToTravel = Vector3.Distance(_startingPos, _targetPos) / SPEED;
                _elapsed = 0f;
                _goToTarget = true;

                transform.LookAt(_targetPos);
            }
        }

        if (_goToTarget)
        {
            _elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(_startingPos, _targetPos, _elapsed / _timeToTravel);

            if (_elapsed >= _timeToTravel)
            {
                _goToTarget = false;
                _elapsed = 0f;
                _timeToTravel = 0f;
            }
        }
    }

    private void OnSwitchTrail(TrailRenderer render)
    {
        DisableAll();
        render.enabled = true;
        ParticleSystem system = render.GetComponent<ParticleSystem>();
        if (system)
        {
            system.Play();
        }
    }

    private void DisableAll()
    {
        foreach (TrailRenderer trailRenderer in _renderers)
        {
            trailRenderer.enabled = false;
            ParticleSystem system = trailRenderer.GetComponent<ParticleSystem>();
            if (system)
            {
                system.Pause();
            }
        }
    }
    
    
}
