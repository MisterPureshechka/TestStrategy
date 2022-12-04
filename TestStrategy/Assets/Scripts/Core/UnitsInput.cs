using System.Linq;
using UnityEngine;

namespace Core
{
    public class UnitsInput : MonoBehaviour
    {
        [SerializeField] private string _controlButtonName = "Fire1";
        [SerializeField] private float _selectUnitRadius = 2f;
        [SerializeField] private int _targetTeam;

        private Unit _selectedUnit;

        private void Update()
        {
            if (!Input.GetButtonUp(_controlButtonName))
                return;

            var inputRay = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(inputRay, out var hit, float.MaxValue, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                return;

            if (_selectedUnit != null)
            {
                _selectedUnit.ChangePath(hit.point);
                _selectedUnit.OnDeselected();
                _selectedUnit = null;
            }
            else
            {
                _selectedUnit = Unit.Units.OrderBy(u => Vector3.Distance(u.transform.position, hit.point))
                    .FirstOrDefault(u => Vector3.Distance(u.transform.position, hit.point) < _selectUnitRadius);
                if (_selectedUnit != null)
                    _selectedUnit.OnSelected();
            }
        }
    }
}