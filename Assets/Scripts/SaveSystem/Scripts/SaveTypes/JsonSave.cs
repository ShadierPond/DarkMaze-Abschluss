using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace GlobalSaveLoad
{
    /// <summary>
    /// <para>This class is used to save and load data from a JSON file.</para>
    /// <para>It uses the JSONUtility class to serialize and deserialize the data.</para>
    /// <para>The Dictionary is generic, so it can be used to save and load any type of object (Except Unity´s Variables, like Vectors, Quaternions, Colors, etc.) .</para>
    /// <para>Methods <c>AddData(), GetData(), RemoveData()</c> only work for generic Dictionary, dont use for DataClass</para>
    /// <para>It Saves Custom Class too, just set it up in the Manager. It uses Custom Converters for Unity´s Variables, so it should save and load anything</para>
    /// </summary>
    [Serializable]
    public class JsonSave
    {
        // The JSON Settings used to serialize and deserialize the data
        private JsonSerializerSettings _settings = new()
        {
            // It contains the Type of the object to be serialized
            TypeNameHandling = TypeNameHandling.All,
            // It formats the JSON file to be more readable
            Formatting = Formatting.Indented,
            // It uses the custom converters to serialize and deserialize Vectors and Quaternions
            Converters = new JsonConverter[] {
                new Vector2Converter(),
                new Vector3Converter(),
                new Vector4Converter(),
                new QuaternionConverter(),
                new ColorConverter()
            }
        };

        
        // Flush the Save Data
        public void ClearData()
        {
            SaveManager.Instance.dataClass = new Data();
        }
        
        // Save the Data to a file with the specified path.
        // The name of the file is will be specified in the path string.
        // Choose in the Parameters if you want to pretty print the JSON file.
        public void Save(string path)
        {
            // Create a new StreamWriter with the specified path
            using StreamWriter writer = new(path);
            var json = JsonConvert.SerializeObject(SaveManager.Instance.dataClass, _settings);
            // Write the JSON string to the file
            writer.Write(json);
            // TODO: Remove this
            Debug.Log($"Saved to {path}");
        }

        // Load the Data from a file with the specified path.
        // The name of the file is will be specified in the path string.
        public void Load(string path)
        {
            // Create a new StreamReader with the specified path
            using StreamReader reader = new(path);
            // Read the JSON string from the file
            var json = reader.ReadToEnd();
            // Deserialize the JSON string to a Data class.
            SaveManager.Instance.dataClass = JsonConvert.DeserializeObject<Data>(json, _settings);
            // TODO: Remove this
            Debug.Log($"Loaded from {path}");
        }
    }
    
    // ----------------------------- CONVERTERS ----------------------------- //
    
    public class Vector4Converter : JsonConverter
    {
        // Check if the object is a Vector4. If it is, it will use the custom converter. Else, it will use the default converter.
        public override bool CanConvert(Type objectType)=>
            objectType == typeof(Vector4);
        // Deserialize the Vector4 from the JSON string
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                // If the token is an Object, it will read the Vector4 from the JSON string
                case JsonToken.StartObject:
                {
                    // Load JObject from stream
                    var obj = JObject.Load(reader);
                    // Create target object based on JObject
                    return new Vector4((float)obj["x"], (float)obj["y"], (float)obj["z"], (float)obj["w"]);
                }
                // If the Object is empty, it will return a new empty Vector4
                case JsonToken.Null:
                    return Vector4.zero;
                // If the token is not an Object, it will throw an exception
                default:
                    throw new JsonException("Unexpected token type: " + reader.TokenType);
            }
        }
        // Serialize the Vector4 to the JSON string
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // If the value is not empty, it will write the Vector4 to a Object in the JSON string
            if (value != null)
            {
                // Get the Vector4 value
                var vector = (Vector4)value;
                // Create a new JObject with the Vector4 values
                var obj = new JObject(
                    new JProperty("x", vector.x),
                    new JProperty("y", vector.y),
                    new JProperty("z", vector.z),
                    new JProperty("w", vector.w)
                );
                // Write the JObject to the JSON string
                obj.WriteTo(writer);
            }
            // If the value is empty, it will write zero values to the JSON string
            else
                JValue.CreateNull().WriteTo(writer);
        }
    }

    public class Vector3Converter : JsonConverter
    {
        // Check if the object is a Vector3. If it is, it will use the Vector3Converter. Else, it will use the default converter.
        public override bool CanConvert(Type objectType)=>
            objectType == typeof(Vector3);
        // Deserialize the Vector3 from the JSON string
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                // If the token is an Object, it will read the Vector3 from the JSON string
                case JsonToken.StartObject:
                {
                    // Load JObject from stream
                    var obj = JObject.Load(reader);
                    // Create target object based on JObject
                    return new Vector3((float)obj["x"], (float)obj["y"], (float)obj["z"]);
                }
                // If the Object is empty, it will return a new empty Vector3
                case JsonToken.Null:
                    return Vector3.zero;
                // If the token is not an Object, it will throw an exception
                default:
                    throw new JsonException("Unexpected token type: " + reader.TokenType);
            }
        }
        // Serialize the Vector3 to the JSON string
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // If the value is not empty, it will write the Vector3 to a Object in the JSON string
            if (value != null)
            {
                // Get the Vector3 value
                var vector = (Vector3)value;
                // Create a new JObject with the Vector3 values
                var obj = new JObject(
                    new JProperty("x", vector.x),
                    new JProperty("y", vector.y),
                    new JProperty("z", vector.z)
                );
                // Write the JObject to the JSON string
                obj.WriteTo(writer);
            }
            // If the value is empty, it will write zero values to the JSON string
            else
                JValue.CreateNull().WriteTo(writer);
        }
    }
    
    public class Vector2Converter : JsonConverter
    {
        // Check if the object is a Vector2. If it is, it will use the Vector2Converter. Else, it will use the default converter.
        public override bool CanConvert(Type objectType)=>
            objectType == typeof(Vector2);
        // Deserialize the Vector2 from the JSON string
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                // If the token is an Object, it will read the Vector2 from the JSON string
                case JsonToken.StartObject:
                {
                    // Load JObject from stream
                    var obj = JObject.Load(reader);
                    // Create target object based on JObject
                    return new Vector2((float)obj["x"], (float)obj["y"]);
                }
                // If the Object is empty, it will return a new empty Vector2
                case JsonToken.Null:
                    return Vector2.zero;
                // If the token is not an Object, it will throw an exception
                default:
                    throw new JsonException("Unexpected token type: " + reader.TokenType);
            }
        }
        // Serialize the Vector2 to the JSON string
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // If the value is not empty, it will write the Vector2 to a Object in the JSON string
            if (value != null)
            {
                // Get the Vector2 value
                var vector = (Vector2)value;
                // Create a new JObject with the Vector2 values
                var obj = new JObject(
                    new JProperty("x", vector.x),
                    new JProperty("y", vector.y)
                );
                // Write the JObject to the JSON string
                obj.WriteTo(writer);
            }
            // If the value is empty, it will write zero values to the JSON string
            else
                JValue.CreateNull().WriteTo(writer);
        }
    }
    
    public class QuaternionConverter : JsonConverter
    {
        // Check if the object is a Quaternion. If it is, it will use the QuaternionConverter. Else, it will use the default converter.
        public override bool CanConvert(Type objectType)=>
            objectType == typeof(Quaternion);
        // Deserialize the Quaternion from the JSON string
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                // If the token is an Object, it will read the Quaternion from the JSON string
                case JsonToken.StartObject:
                {
                    // Load JObject from stream
                    var obj = JObject.Load(reader);
                    // Create target object based on JObject
                    return new Quaternion((float)obj["x"], (float)obj["y"], (float)obj["z"], (float)obj["w"]);
                }
                // If the Object is empty, it will return a new empty Quaternion
                case JsonToken.Null:
                    return Quaternion.identity;
                // If the token is not an Object, it will throw an exception
                default:
                    throw new JsonException("Unexpected token type: " + reader.TokenType);
            }
        }
        // Serialize the Quaternion to the JSON string
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // If the value is not empty, it will write the Quaternion to a Object in the JSON string
            if (value != null)
            {
                // Get the Quaternion value
                var quaternion = (Quaternion)value;
                // Create a new JObject with the Quaternion values
                var obj = new JObject(
                    new JProperty("x", quaternion.x),
                    new JProperty("y", quaternion.y),
                    new JProperty("z", quaternion.z),
                    new JProperty("w", quaternion.w)
                );
                // Write the JObject to the JSON string
                obj.WriteTo(writer);
            }
            // If the value is empty, it will write zero values to the JSON string
            else
                JValue.CreateNull().WriteTo(writer);
        }
    }
    
    public class ColorConverter : JsonConverter
    {
        // Check if the object is a Color. If it is, it will use the ColorConverter. Else, it will use the default converter.
        public override bool CanConvert(Type objectType)=>
            objectType == typeof(Color);
        // Deserialize the Color from the JSON string
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                // If the token is an Object, it will read the Color from the JSON string
                case JsonToken.StartObject:
                {
                    // Load JObject from stream
                    var obj = JObject.Load(reader);
                    // Create target object based on JObject
                    return new Color((float)obj["r"], (float)obj["g"], (float)obj["b"], (float)obj["a"]);
                }
                // If the Object is empty, it will return a new empty Color
                case JsonToken.Null:
                    return Color.clear;
                // If the token is not an Object, it will throw an exception
                default:
                    throw new JsonException("Unexpected token type: " + reader.TokenType);
            }
        }
        // Serialize the Color to the JSON string
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // If the value is not empty, it will write the Color to a Object in the JSON string
            if (value != null)
            {
                // Get the Color value
                var color = (Color)value;
                // Create a new JObject with the Color values
                var obj = new JObject(
                    new JProperty("r", color.r),
                    new JProperty("g", color.g),
                    new JProperty("b", color.b),
                    new JProperty("a", color.a)
                );
                // Write the JObject to the JSON string
                obj.WriteTo(writer);
            }
            // If the value is empty, it will write zero values to the JSON string
            else
                JValue.CreateNull().WriteTo(writer);
        }
    }
}