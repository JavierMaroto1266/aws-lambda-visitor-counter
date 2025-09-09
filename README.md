AWS Lambda + API Gateway Visitor Counter

A serverless visitor counter built with AWS Lambda (C# .NET), API Gateway, and DynamoDB.

Architecture

•	API Gateway: REST endpoint at /visit

•	Lambda: C# .NET function that increments the visitor counter

•	DynamoDB: Persistent storage for the visitor count

How It Works

1\.	Client sends an HTTP GET request to the API Gateway endpoint

2\.	API Gateway triggers the Lambda function

3\.	Lambda retrieves the current count from DynamoDB

4\.	Increments the count by 1

5\.	Writes the updated count back to DynamoDB

6\.	Returns a JSON response containing the current visitor count

Live Demo

Endpoint:

https://027geh8g52.execute-api.us-east-1.amazonaws.com/prod/visit

Test it — each request increments the counter:

Bash:

curl https://027geh8g52.execute-api.us-east-1.amazonaws.com/prod/visit

AWS Services Used

•	AWS Lambda (C# .NET 8.0)

•	Amazon API Gateway (REST API)

•	Amazon DynamoDB (NoSQL database)

•	AWS IAM (Permissions and execution roles)

Setup Instructions

1\. DynamoDB Table

•	Table Name: visitor\_counter

•	Primary Key: id (String)

•	Initial Item:

Json:

{"id": "counter", "count": 0}

2\. Lambda Function

•	Runtime: .NET 8

•	Handler: VisitorCounterFunction::VisitorCounterFunction.Function::FunctionHandler

•	IAM Role: Must include permissions to read/write to the DynamoDB table

3\. API Gateway

•	Create a REST API with a /visit resource

•	Configure a GET method integrated with the Lambda function

•	Enable CORS to allow browser-based requests

Code Features

•	Graceful error handling for missing or malformed DynamoDB records

•	CORS headers included for web compatibility

•	Async/await pattern for optimal performance

•	Clean, consistent JSON response format

Future Enhancements

•	Build a frontend dashboard to display visitor stats

•	Implement API rate limiting via API Gateway or Lambda

•	Integrate CloudWatch metrics and alarms for monitoring

•	Automate deployment using Terraform or AWS SAM



