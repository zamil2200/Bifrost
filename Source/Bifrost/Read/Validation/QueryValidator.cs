﻿#region License
//
// Copyright (c) 2008-2014, Dolittle (http://www.dolittle.com)
//
// Licensed under the MIT License (http://opensource.org/licenses/MIT)
//
// You may not use this file except in compliance with the License.
// You may obtain a copy of the license at 
//
//   http://github.com/dolittle/Bifrost/blob/master/MIT-LICENSE.txt
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion
using System;
using System.Linq.Expressions;
using Bifrost.Rules;
using Bifrost.Extensions;
using System.Reflection;

namespace Bifrost.Read.Validation
{
#pragma warning disable 1591 // Xml Comments
    public static class MethodCalls
    {
        public static TOut CallGenericMethod<TOut, T, T1, T2, T3>(this T target, Expression<Func<T, Func<T1, T2, T3, TOut>>> method, T1 param1, T2 param2, T3 param3, params Type[] genericArguments)
        {
            return CallGenericMethod<T, TOut>(target, method, new object[] { param1, param2, param3 }, genericArguments);
        }

        public static TOut CallGenericMethod<TOut, T, T1, T2>(this T target, Expression<Func<T, Func<T1, T2, TOut>>> method, T1 param1, T2 param2, params Type[] genericArguments)
        {
            return CallGenericMethod<T, TOut>(target, method, new object[] { param1, param2 }, genericArguments);
        }

        public static TOut CallGenericMethod<TOut, T, T1>(this T target, Expression<Func<T, Func<T1, TOut>>> method, T1 param1, params Type[] genericArguments)
        {
            return CallGenericMethod<T, TOut>(target, method, new object[] { param1 }, genericArguments);
        }

        public static TOut CallGenericMethod<TOut, T>(this T target, Expression<Func<T, Func<TOut>>> method, params Type[] genericArguments)
        {
            return CallGenericMethod<T, TOut>(target, method, new object[0], genericArguments);
        }

        static TOut CallGenericMethod<T, TOut>(this T target, Expression method, object[] parameters, Type[] genericArguments)
        {
            var lambda = method as LambdaExpression;
            var unary = lambda.Body as UnaryExpression;
            var methodCall = unary.Operand as MethodCallExpression;
            var constant = methodCall.Object as ConstantExpression;

            var methodInfo = constant.Value as MethodInfo;
            var genericMethodDefinition = methodInfo.GetGenericMethodDefinition();

            var genericMethod = genericMethodDefinition.MakeGenericMethod(genericArguments);

            var result = genericMethod.Invoke(target, parameters);
            return (TOut)result;
        }

        public class Generify<T>
        {
            public TOut Invoke<TOut>(Expression<Func<T, Func<TOut>>> method, params Type[] genericArguments)
            {
                throw new NotImplementedException();
            }

            public TOut Invoke<TOut, TParam1>(Expression<Func<T, Func<TParam1, TOut>>> method, TParam1 param1, params Type[] genericArguments)
            {
                throw new NotImplementedException();
            }
        }

        public static Generify<T> Generics<T>(this T g)
        {
            return new Generify<T>();
        }
    }

    public class ClassWithGenericMethods
    {
        public object Something<T>()
        {
            return null;
        }

        public object Something<T>(int param1)
        {
            return null;
        }


        public object Something<T>(int param1, double param2, string param3)
        {
            return null;
        }
        
    }
#pragma warning restore 1591 // Xml Comments

    /// <summary>
    /// Represents an implementation of <see cref="IQueryValidator"/>
    /// </summary>
    public class QueryValidator : IQueryValidator
    {
        IQueryValidationDescriptors _descriptors;

        /// <summary>
        /// Initializes an instance of <see cref="QueryValidator"/>
        /// </summary>
        /// <param name="descriptors"><see cref="IQueryValidationDescriptors"/> for getting descriptors for queries for running through rules</param>
        public QueryValidator(IQueryValidationDescriptors descriptors)
        {
            _descriptors = descriptors;
        }

#pragma warning disable 1591 // Xml Comments
        public QueryValidationResult Validate(IQuery query)
        {
            var result = new QueryValidationResult(new BrokenRule[0]);

            //var c = new ClassWithGenericMethods();
            //c.Generics().Invoke<object>(m => m.Something<object>, typeof(string));
            //c.CallGenericMethod<bool, ClassWithGenericMethods, int, double, string>(d => d.Something<object>, 42, 42.0, "fourty two", typeof(string));

            IRuleContext

            var hasDescriptor = _descriptors.CallGenericMethod<bool, IQueryValidationDescriptors>(d => d.HasDescriptorFor<IQuery>, query.GetType());
            if (hasDescriptor)
            {
                var descriptor = _descriptors.CallGenericMethod<IQueryValidationDescriptor, IQueryValidationDescriptors>(d => d.GetDescriptorFor<IQuery>, query.GetType());
                descriptor.ArgumentRules.ForEach(r=>r.Evaluate())

            }

            return result;
        }
#pragma warning restore 1591 // Xml Comments
    }
}
