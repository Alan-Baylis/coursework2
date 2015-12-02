using UnityEngine;
using System.Collections;

public interface IConnectable<T> {
    bool Connect(T other);
}
