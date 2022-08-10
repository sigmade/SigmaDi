using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SigmaDi
{
    public class Container
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public Container(string servicesFile)
        {
            RegisterByFile(servicesFile);
        }

        public void AddNew<TInterface, TImplement>() where TImplement : TInterface
        {
            var instanceService = GetInstance(typeof(TImplement));
            _services.Add(typeof(TInterface), instanceService);
        }

        public object GetInstance(Type type)
        {
            if (_services.TryGetValue(type, out var instance))
            {
                return instance;
            }
            else if (!type.IsAbstract)
            {
                return CreateInstance(type);
            }
            throw new Exception($"Failed registration: {type}");
        }

        private object CreateInstance(Type type)
        {
            var ctor = type.GetConstructors().Single();
            var ctorParamsInfo = ctor.GetParameters();
            var ctorParamTypes = ctorParamsInfo.Select(p => p.ParameterType);
            var ctorParamInstances = new List<object>();

            foreach (var ctorParamType in ctorParamTypes)
            {
                var ctorParamInstance = GetInstance(ctorParamType);
                ctorParamInstances.Add(ctorParamInstance);
            }

            var instance = Activator.CreateInstance(type, ctorParamInstances.ToArray());
            return instance;
        }

        private void RegisterByFile(string servicesFile)
        {
            var data = "";

            using (var reader = new StreamReader(servicesFile))
            {
                data = reader.ReadToEnd();
            }

            var services = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);

            foreach (var service in services)
            {
                var interfaceType = Type.GetType(service.Key);
                var type = Type.GetType(service.Value);
                var instance = GetInstance(type);

                _services.Add(interfaceType, instance);
            }
        }
    }
}
