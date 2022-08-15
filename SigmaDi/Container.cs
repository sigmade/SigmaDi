using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SigmaDi
{
    public class Container
    {
        private readonly Dictionary<Type, Type> _servicesType = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public Container(string servicesFile)
        {
            RegisterByFile(servicesFile);
        }

        public void AddNew<TInterface, TImplement>() where TImplement : TInterface
        {
            _servicesType.Add(typeof(TInterface), typeof(TImplement));
        }

        public void CreateDependencies()
        {
            foreach (var service in _servicesType)
            {
                var instanceService = GetInstance(service.Value);
                _services.Add(service.Key, instanceService);
            }
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
            else if (_servicesType.TryGetValue(type, out var typeService))
            {
                return CreateInstance(typeService);
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

            var services = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(data);

            foreach (var service in services)
            {
                RegisterServicesByAssembly(service.Key, service.Value);
            }
        }

        private void RegisterServicesByAssembly(string assemblyName, Dictionary<string, string> services)
        {
            foreach (var service in services)
            {
                var interfaceType = Type.GetType($"{assemblyName}.{service.Key}, {assemblyName}");
                var type = Type.GetType($"{assemblyName}.{service.Value}, {assemblyName}");

                _servicesType.Add(interfaceType, type);
            }
        }
    }
}
