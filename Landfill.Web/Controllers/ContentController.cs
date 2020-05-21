﻿using Landfill.BAL.Abstract;
using Landfill.Entities;
using Landfill.Models;
using Lanfill.BAL;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using static Landfill.Common.Enums.EnumsContainer;

namespace Landfill.Web.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    ///[ODataRoutePrefix("api")]
    public partial class ContentController : ControllerBase
    {
        private readonly IContentService contentService;

        public ContentController(IContentService contentService)
        {
            this.contentService = contentService;
        }
        // GET: api/Content

        //[HttpGet]
        //[EnableQuery()]
        //public ActionResult<IEnumerable<Content>> Get()
        //{
        //    var list = contentService.GetAllConntent();
        //    //return await landfillContext.Contents.ToListAsync();
        //    return Ok(list);
        //}
        //string -> Odata -(DTO -> to tables> -?  Dictionary<TTable,IQueryAble<TTable>> Iqueryable<Content> -> 
        //стрінг конвертується в Iqueryable<Content>
        //в механізм IQuerayable для OData 
        //Взять метод мапера і перетоврити моделі на аентівті і виконати запити вже по них

        //Робочий метод
        //[HttpGet]
        //[Route("contentModel")]
        //public ActionResult GetContentByIdWithTranslation(int contentId, ContentType contentType)
        //{
        //    var data = contentService.GetContentByIdWithTranslation(contentId, contentType);
        //    return Ok(data);
        //}


        [HttpGet]
        [EnableQuery()]
       // [ODataRoute("Contents")]
        public ActionResult<IQueryable<ContentDto>> Get(ODataQueryOptions<ContentDto> options)
        { // This is the trick to get the expression out of the FilterQueryOption...
            //IQueryable queryable = Enumerable.Empty<ContentDto>().AsQueryable();
            var queryable = options.OrderBy;
            //var kkk = options.OrderBy.ApplyTo()
            //var exp = (MethodCallExpression)queryable.Expression;              // <-- This comes back as a MethodCallExpression...

            //// Map the expression to my intermediate Product object type
            //var mappedExp = mapper.Map<Expression<Func<Product, bool>>>(exp);   // <-- But I want it as a Expression<Func<ProductDTO, bool>> so I can map it...

            //IEnumerable<Product> results = _dataAccessLayer.GetProducts(mappedExp);

            //return mapper.Map<IEnumerable<ProductDTO>>(results);

            var data = contentService.GetAllContent();
            return Ok(data);
        }





        //public async Task<ActionResult<IEnumerable<Content>>> Get(int contentId, ContentType contentType)
        //{
        //    return await landfillContext.Contents.Where(e => e.ContentType == contentType).ToListAsync();
        //}
        //public class DictionaryTKeyEnumTValueConverter : JsonConverterFactory
        //{
        //    public override bool CanConvert(Type typeToConvert)
        //    {
        //        if (!typeToConvert.IsGenericType)
        //        {
        //            return false;
        //        }

        //        if (typeToConvert.GetGenericTypeDefinition() != typeof(Dictionary<,>))
        //        {
        //            return false;
        //        }

        //        return typeToConvert.GetGenericArguments()[0].IsEnum;
        //    }

        //    public override JsonConverter CreateConverter(
        //        Type type,
        //        JsonSerializerOptions options)
        //    {
        //        Type keyType = type.GetGenericArguments()[0];
        //        Type valueType = type.GetGenericArguments()[1];

        //        JsonConverter converter = (JsonConverter)Activator.CreateInstance(
        //            typeof(DictionaryEnumConverterInner<,>).MakeGenericType(
        //                new Type[] { keyType, valueType }),
        //            BindingFlags.Instance | BindingFlags.Public,
        //            binder: null,
        //            args: new object[] { options },
        //            culture: null);

        //        return converter;
        //    }

        //    private class DictionaryEnumConverterInner<TKey, TValue> :
        //        JsonConverter<Dictionary<TKey, TValue>> where TKey : struct, Enum
        //    {
        //        private readonly JsonConverter<TValue> _valueConverter;
        //        private Type _keyType;
        //        private Type _valueType;

        //        public DictionaryEnumConverterInner(JsonSerializerOptions options)
        //        {
        //            // For performance, use the existing converter if available.
        //            _valueConverter = (JsonConverter<TValue>)options
        //                .GetConverter(typeof(TValue));

        //            // Cache the key and value types.
        //            _keyType = typeof(TKey);
        //            _valueType = typeof(TValue);
        //        }

        //        public override Dictionary<TKey, TValue> Read(
        //            ref Utf8JsonReader reader,
        //            Type typeToConvert,
        //            JsonSerializerOptions options)
        //        {
        //            if (reader.TokenType != JsonTokenType.StartObject)
        //            {
        //                throw new JsonException();
        //            }

        //            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

        //            while (reader.Read())
        //            {
        //                if (reader.TokenType == JsonTokenType.EndObject)
        //                {
        //                    return dictionary;
        //                }

        //                // Get the key.
        //                if (reader.TokenType != JsonTokenType.PropertyName)
        //                {
        //                    throw new JsonException();
        //                }

        //                string propertyName = reader.GetString();

        //                // For performance, parse with ignoreCase:false first.
        //                if (!Enum.TryParse(propertyName, ignoreCase: false, out TKey key) &&
        //                    !Enum.TryParse(propertyName, ignoreCase: true, out key))
        //                {
        //                    throw new JsonException(
        //                        $"Unable to convert \"{propertyName}\" to Enum \"{_keyType}\".");
        //                }

        //                // Get the value.
        //                TValue v;
        //                if (_valueConverter != null)
        //                {
        //                    reader.Read();
        //                    v = _valueConverter.Read(ref reader, _valueType, options);
        //                }
        //                else
        //                {
        //                    v = JsonSerializer.Deserialize<TValue>(ref reader, options);
        //                }

        //                // Add to dictionary.
        //                dictionary.Add(key, v);
        //            }

        //            throw new JsonException();
        //        }

        //        public override void Write(
        //            Utf8JsonWriter writer,
        //            Dictionary<TKey, TValue> dictionary,
        //            JsonSerializerOptions options)
        //        {
        //            writer.WriteStartObject();

        //            foreach (KeyValuePair<TKey, TValue> kvp in dictionary)
        //            {
        //                writer.WritePropertyName(kvp.Key.ToString());

        //                if (_valueConverter != null)
        //                {
        //                    _valueConverter.Write(writer, kvp.Value, options);
        //                }
        //                else
        //                {
        //                    JsonSerializer.Serialize(writer, kvp.Value, options);
        //                }
        //            }

        //            writer.WriteEndObject();
        //        }
        //    }
    }
    }



