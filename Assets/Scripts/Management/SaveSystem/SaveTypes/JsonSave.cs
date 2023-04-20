using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Management.SaveSystem.SaveTypes
{
    [Serializable]
    public class JsonSave
    {
        private SaveManager _saveManager;
        public JsonSave(SaveManager saveManager)
        => _saveManager = saveManager;

        /// <summary>
        /// A field that holds the settings for JSON serialization and deserialization.
        /// </summary>
        /// <remarks>
        /// It requires that the Newtonsoft.Json library is imported and that the custom converters for Vectors, Quaternions and Colors are defined.
        /// </remarks>
        private JsonSerializerSettings _settings = new()
        {
            // It specifies that the Type of the object to be serialized should be included in the JSON output, to preserve polymorphism and inheritance
            TypeNameHandling = TypeNameHandling.All,
            // It formats the JSON output to be indented and human-readable
            Formatting = Formatting.Indented,
            // It uses the custom converters to handle the serialization and deserialization of Vectors, Quaternions and Colors, which are not natively supported by JSON
            Converters = new JsonConverter[] {
                new Vector2Converter(),
                new Vector3Converter(),
                new Vector4Converter(),
                new QuaternionConverter(),
                new ColorConverter()
            }
        };

        /// <summary>
        /// A method that saves the settings data stored in the SaveManager instance to a file in JSON format.
        /// </summary>
        /// <param name="path">string - The path of the file to save to.</param>
        /// <remarks>
        /// It requires that the SaveManager class is defined and that it has a static property called Instance that returns a singleton instance of the class. It also requires that the SettingsData class is defined and that it can be serialized and deserialized using JSON. It also requires that the _settings field is defined and that it holds the settings for JSON serialization and deserialization.
        /// </remarks>
        public void SaveSettings(string path)
        {
            // Creates a new StreamWriter with the specified path.
            using StreamWriter writer = new(path);
            // Serializes the settingsDataClass property of the SaveManager instance to a JSON string, using the _settings field.
            var json = JsonConvert.SerializeObject(_saveManager.settingsDataClass, _settings);
            // Writes the JSON string to the file.
            writer.Write(json);
        }
        
        public void LoadSettings(string path)
        {
            // Create a new StreamReader with the specified path
            using StreamReader reader = new(path);
            // Read the JSON string from the file
            var json = reader.ReadToEnd();
            // Deserialize the JSON string to a Data class.
            _saveManager.settingsDataClass = JsonConvert.DeserializeObject<SettingsData>(json, _settings);
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