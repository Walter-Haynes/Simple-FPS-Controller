using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using JetBrains.Annotations;

namespace CommonGames.Utilities.CGTK
{
    public sealed partial class CGTelegraph
    {
        #region Normal Receivers
        
        [PublicAPI]
        public void Add(in IReceiver receiver, in Type type)
        {
            if(Signals.TryGetValue(type.GetHashCode(), out List<IReceiver> __existingSignals))
            {
                //If the Dictionary contains a list of this type of receiver. Add this receiver to the the list.
                __existingSignals.Add(receiver);
            }
            else
            {
                //If not, add an entry list into the Dictionary for this type of receiver.
                Signals.Add(type.GetHashCode(), new List<IReceiver> {receiver});   
            }
        }

        [PublicAPI]
        public void Remove(in IReceiver receiver, in Type type)
        {
            if(Signals.TryGetValue(type.GetHashCode(), out List<IReceiver> __cachedSignals))
            {
                __cachedSignals.Remove(receiver);
            }
        }
        
        #endregion

        #region Inheriting Receivers

        [PublicAPI]
        public void Add(in object obj)
        {
            //Gets all Interfaces on the object.
            Type[] __all = obj.GetType().GetInterfaces();
            //Tries to cast the object to an IReceiver.
            IReceiver __receiver = obj as IReceiver;
                
            foreach(Type __interfaceType in __all)
            {
                if(__interfaceType.IsGenericType && (__interfaceType.GetGenericTypeDefinition() == typeof(IReceiver<>)))
                {
                    //If the interface is the correct type, call the regular function and receive the type for the generic Arguments.
                    Add(
                        receiver: __receiver, 
                        type: __interfaceType.GetGenericArguments()[0]);
                    return;
                }
            }
        }

        [PublicAPI]
        public void Remove(in object obj)
        {
            //Gets all Interfaces on the object.
            Type[] __all = obj.GetType().GetInterfaces();
            //Tries to cast the object to an IReceiver.
            IReceiver __receiver = obj as IReceiver;
            
            foreach(Type __interfaceType in __all)
            {
                if(__interfaceType.IsGenericType && (__interfaceType.GetGenericTypeDefinition() == typeof(IReceiver<>)))
                {
                    //If the interface is the correct type, call the regular function and receive the type for the generic Arguments.
                    Remove(
                        receiver: __receiver, 
                        type: __interfaceType.GetGenericArguments()[0]);
                    return;
                }
            }
        }

        #endregion
    }
}