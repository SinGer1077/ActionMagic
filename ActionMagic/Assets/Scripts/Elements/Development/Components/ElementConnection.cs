using UnityEngine;

using Unity.Collections;
using Unity.Entities;

using Elements.Components;

namespace Elements.Data
{
    public struct ElementConnection : IBufferElementData
    {
        public readonly int id;
        public readonly BaseElementComponent ConnectedElement;
        public Vector3 ConnectionPosition;
        public bool IsReacted;

        public ElementConnection (BaseElementComponent element)
        {
            id = element.id.Index;
            ConnectedElement = element;
            IsReacted = false;
            ConnectionPosition = Vector3.zero;
        }

        public ElementConnection(BaseElementComponent element, Vector3 position)
        {
            id = element.id.Index;
            ConnectedElement = element;
            IsReacted = false;
            ConnectionPosition = position;

        }

        public ElementConnection (ElementConnection connection, bool isReacted)
        {
            id = connection.id;
            ConnectedElement = connection.ConnectedElement;
            ConnectionPosition = connection.ConnectionPosition;
            IsReacted = isReacted;
        }
    }
}
