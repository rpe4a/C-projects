using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace JsonParser
{

    internal class QueryFilter
    {
        public QueryFilter(string prop) : this(prop, new HashSet<QueryFilter>()) { }
        public QueryFilter(string prop, HashSet<QueryFilter> nestedFilters)
        {
            Property = prop;
            NestedFilters = nestedFilters;
        }
        public string Property { get; }
        public HashSet<QueryFilter> NestedFilters { get; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var query = "link,property(body(head(description,name),tag),name),description,item(description, name)";

            var jtoken = JToken.Parse(JsonStringObject);

            var jo = new JObject()
            {
                {"object", jtoken }
            };

            Console.WriteLine(jo);

            //Console.WriteLine(jtoken.ToString());
            Console.WriteLine();
            Console.WriteLine(query);
            Console.WriteLine();



            FilterJsonProperties(jo, query);

            Console.WriteLine(jo.First.First.ToString());
            Console.ReadKey();
        }

        private static void FilterJsonProperties(JToken jtoken, string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return;

            if (!(jtoken is JContainer)) return;

            FilterJsonConteiner(jtoken, GetQueryFilters(query));
        }

        private static HashSet<QueryFilter> GetQueryFilters(string queryString)
        {
            try
            {
                var jsonQuery = JToken.Parse(ParseStringToJsonString(queryString));

                Console.WriteLine(jsonQuery);

                return new HashSet<QueryFilter>(GetFilters(jsonQuery).SelectMany(f => f));
            }
            catch
            {
                return null;
            }
        }
        private static string ParseStringToJsonString(string queryString)
        {
            queryString = @"{" + queryString + "}";
            return queryString.Replace(",", ":'',").Replace("(", ":{").Replace(")", ":''}").Replace("}:''", "}");
        }

        private static IEnumerable<IEnumerable<QueryFilter>> GetFilters(JToken element)
        {
            foreach (var el in element.Children())
            {
                var filter = GetFilter(el);

                if (filter != null)
                    yield return filter;
            }
        }

        private static IEnumerable<QueryFilter> GetFilter(JToken el)
        {
            if (el is JProperty property)
            {
                yield return new QueryFilter(property.Name, new HashSet<QueryFilter>(GetFilters(el).SelectMany(f => f)));
            }

            if (el is JObject jObj)
            {
                foreach (var ele in jObj.Children())
                {
                    yield return GetFilter(ele).FirstOrDefault();
                }
            }
        }

        //private static HashSet<QueryFilter> ParseQuery(string queryString)
        //{
        //    var filters = new HashSet<QueryFilter>();

        //    var parameters = queryString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        //    parameters.ForEach(p =>
        //    {
        //        var delimeterOf = p.IndexOf(":", StringComparison.Ordinal);

        //        if (delimeterOf <= 0)
        //            filters.Add(new QueryFilter(p));
        //        else
        //        {
        //            var nestedParameters = p.Substring(delimeterOf + 1, p.Length - delimeterOf - 1)
        //                .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        //            filters.Add(new QueryFilter(
        //                p.Substring(0, delimeterOf),
        //                new HashSet<QueryFilter>(nestedParameters.Select(n => new QueryFilter(n)))
        //            ));
        //        }
        //    });

        //    return filters;
        //}

        private static void FilterJsonConteiner(JToken jtoken, HashSet<QueryFilter> filterFields)
        {
            if (filterFields.Count == 0) return;

            if (!(jtoken is JContainer jContainer)) return;

            foreach (var el in jContainer.Children())
            {
                FilterJsonToken(el, filterFields);
            }
        }

        private static void FilterJsonToken(JToken el, HashSet<QueryFilter> filterFields)
        {
            if (el is JProperty)
                FilterJsonConteiner(el, filterFields);

            if (el is JObject jObj)
            {
                var propNames = jObj.Properties().Select(x => x.Name).ToList();

                propNames.ForEach(p =>
                {
                    var filter = filterFields.FirstOrDefault(f => f.Property == p);

                    if (filter == null)
                        jObj.Property(p).Remove();
                    else
                        FilterJsonConteiner(jObj.Property(p), filter.NestedFilters);
                });
            }

            if (el is JArray jArray)
                FilterJsonConteiner(jArray, filterFields);
        }

        private const string JsonStringObject = @"{
                                                    'title': 'Star Wars',
                                                    'link': 'http://www.starwars.com',
                                                    'property': {
                                                        'name' : 'test_channel',
                                                        'body' : {
                                                              'example': 'eqweqeq',
                                                              'head': {
                                                                'name' : 'test_item',
                                                                'body' : 'bla-bla',
                                                                'description' : 'bla-bla-bla'
                                                                 },
                                                              'tag': 'tag1',
                                                        },
                                                        'description' : 'bla-bla-bla'
                                                    },            
                                                    'description': 'Star Wars blog.',
                                                    'obsolete': 'Obsolete value',
                                                    'item': [{
                                                        'name' : 'test_item',
                                                        'body' : 'bla-bla',
                                                        'description' : 'bla-bla-bla'
                                                         },
                                                        {
                                                        'name' : 'test_item2',
                                                        'body' : 'bla-bla',
                                                        'description' : 'bla-bla-bla'
                                                         }
                                                    ]
                                                }";


        private const string JsonStringArray = @"[{
                                                    'title': 'Star Wars',
                                                    'link': 'http://www.starwars.com',
                                                    'property': {
                                                        'name' : 'test_channel',
                                                        'body' : {
                                                              'example': 'eqweqeq',
                                                              'head': {
                                                                'name' : 'test_item',
                                                                'body' : 'bla-bla',
                                                                'description' : 'bla-bla-bla'
                                                                 },
                                                              'tag': 'tag1',
                                                        },
                                                        'description' : 'bla-bla-bla'
                                                    },            
                                                    'description': 'Star Wars blog.',
                                                    'obsolete': 'Obsolete value',
                                                    'item': [{
                                                        'name' : 'test_item',
                                                        'body' : 'bla-bla',
                                                        'description' : 'bla-bla-bla'
                                                         },
                                                        {
                                                        'name' : 'test_item2',
                                                        'body' : 'bla-bla',
                                                        'description' : 'bla-bla-bla'
                                                         }
                                                    ]
                                                },
{
                                                    'title': 'Star Wars',
                                                    'link': 'http://www.starwars.com',
                                                    'property': {
                                                        'name' : 'test_channel',
                                                        'body' : {
                                                              'example': 'eqweqeq',
                                                              'head': {
                                                                'name' : 'test_item',
                                                                'body' : 'bla-bla',
                                                                'description' : 'bla-bla-bla'
                                                                 },
                                                              'tag': 'tag1',
                                                        },
                                                        'description' : 'bla-bla-bla'
                                                    },            
                                                    'description': 'Star Wars blog.',
                                                    'obsolete': 'Obsolete value',
                                                    'item': [{
                                                        'name' : 'test_item',
                                                        'body' : 'bla-bla',
                                                        'description' : 'bla-bla-bla'
                                                         },
                                                        {
                                                        'name' : 'test_item2',
                                                        'body' : 'bla-bla',
                                                        'description' : 'bla-bla-bla'
                                                         }
                                                    ]
                                                }
]";
    }



}
