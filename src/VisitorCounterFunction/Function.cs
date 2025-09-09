using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace VisitorCounterFunction;

public class Function
{
    // Initialize the DynamoDB client
    private static readonly AmazonDynamoDBClient dynamoDbClient = new AmazonDynamoDBClient();
    private const string TableName = "visitor_counter"; // Our DynamoDB table name

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        // 1. GET THE CURRENT COUNT FROM DYNAMODB
        var getItemRequest = new GetItemRequest
        {
            TableName = TableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "id", new AttributeValue { S = "counter" } } // Look for item with id "counter"
            }
        };

        var getItemResponse = await dynamoDbClient.GetItemAsync(getItemRequest);

        // Check if the item was found and get the current count
        int currentCount = 0;
        if (getItemResponse.Item != null && getItemResponse.Item.ContainsKey("count"))
        {
            currentCount = int.Parse(getItemResponse.Item["count"].N);
        }

        // 2. INCREMENT THE COUNT
        int newCount = currentCount + 1;

        // 3. SAVE THE NEW COUNT BACK TO DYNAMODB
        var putItemRequest = new PutItemRequest
        {
            TableName = TableName,
            Item = new Dictionary<string, AttributeValue>
            {
                { "id", new AttributeValue { S = "counter" } }, // The partition key
                { "count", new AttributeValue { N = newCount.ToString() } } // The new value
            }
        };

        await dynamoDbClient.PutItemAsync(putItemRequest);

        // 4. CREATE THE JSON RESPONSE
        var responseBody = new { count = newCount };
        var jsonResponse = JsonConvert.SerializeObject(responseBody);

        // 5. RETURN THE RESPONSE (API Gateway expects this specific format)
        return new APIGatewayProxyResponse
        {
            StatusCode = 200,
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Access-Control-Allow-Origin", "*" }, // Important: Allows browsers to call this API
                { "Access-Control-Allow-Headers", "Content-Type" },
                { "Access-Control-Allow-Methods", "OPTIONS,GET" }
            },
            Body = jsonResponse
        };
    }
}