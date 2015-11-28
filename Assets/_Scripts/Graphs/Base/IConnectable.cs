using UnityEngine;
using System.Collections;

public interface IConnectable<T> {
    void Connect(T other);
}
