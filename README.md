# gRPC microservice

About using gRPC in microservices for building a high performance interservice communication with .NET6

## Overall Picture
See the overall picture of real-world **e-commerce microservices** project. You can see that, there are 6 microservices.
I use worker services and .NET 6 gRPC applications to build client and server gRPC components defining proto service definition contracts.

![Overall Picture of Repository](https://user-images.githubusercontent.com/1147445/98652230-5f66ee80-234c-11eb-9201-8b291b331c9f.png)

Basically, I implement e-commerce logic with only gRPC communication. There are 3 gRPC server applications which are `product`, `shopping cart` and `discount` gRPC services & there are 2 worker services which are `product worker service` and `shopping cart worker service`. Worker services are client and perform operations over the gRPC server applications & it secure the gRPC services with standalone `identity server` microservices with OAuth 2.0 and JWT token.
