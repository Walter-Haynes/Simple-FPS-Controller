using System.Collections.Generic;
using UnityEngine;

//Written by Wybren
public class UUID : MonoBehaviour, ISerializationCallbackReceiver
{
    static Dictionary<UUID, string> StringToUUID = new Dictionary<UUID, string>();
    static Dictionary<string, UUID> UUIDToString = new Dictionary<string, UUID>();

    static void RegisterUUID(UUID self)
    {
        string UID;
        if (StringToUUID.TryGetValue(self, out UID))
        {
            // found object instance, update ID
            self.p_UUID = UID;
            self.p_IDBackup = self.p_UUID;
            if (!UUIDToString.ContainsKey(UID))
            {
                UUIDToString.Add(UID, self);
            }

            return;
        }

        if (string.IsNullOrEmpty(self.p_UUID))
        {
            // No ID yet, generate a new one.
            self.p_UUID = System.Guid.NewGuid().ToString();
            self.p_IDBackup = self.p_UUID;
            UUIDToString.Add(self.p_UUID, self);
            StringToUUID.Add(self, self.p_UUID);

            return;
        }

        UUID tmp;
        if (!UUIDToString.TryGetValue(self.p_UUID, out tmp))
        {
            // ID not known to the Database, so just register it
            UUIDToString.Add(self.p_UUID, self);
            StringToUUID.Add(self, self.p_UUID);

            return;
        }
        if (tmp == self)
        {
            // Database inconsistency
            StringToUUID.Add(self, self.p_UUID);

            return;
        }
        if (tmp == null)
        {
            // object in Database got destroyed, replace with new
            UUIDToString[self.p_UUID] = self;
            StringToUUID.Add(self, self.p_UUID);

            return;
        }

        // we got a duplicate, generate new ID
        self.p_UUID = System.Guid.NewGuid().ToString();
        self.p_IDBackup = self.p_UUID;
        UUIDToString.Add(self.p_UUID, self);
        StringToUUID.Add(self, self.p_UUID);
    }

    static void UnregisterUUID(UUID self)
    {
        UUIDToString.Remove(self.p_UUID);
        StringToUUID.Remove(self);
    }

    [SerializeField] private string p_UUID = null;
    private string p_IDBackup = null;

    /// <summary>
    /// Returns the ID of the object.
    /// </summary>
    public string ID { get { return p_UUID; } }

    public void OnAfterDeserialize()
    {
        if (p_UUID == null || p_UUID != p_IDBackup)
            RegisterUUID(this);
    }
    public void OnBeforeSerialize()
    {
        if (p_UUID == null || p_UUID != p_IDBackup)
            RegisterUUID(this);
    }
    void OnDestroy()
    {
        UnregisterUUID(this);
        p_UUID = null;
    }
}
