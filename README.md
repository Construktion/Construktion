What is Construktion
---
Construktion is a library created to assist with unit testing by simplifying the [arrange](http://wiki.c2.com/?ArrangeActAssert) portion of your tests.
Its role is to create objects and populate their properties with random data. Many times while testing we don't care what the data is, only that it's _something_.
  
Getting started
---
Objects are built using the Construktion class.

```c#
var construktion = new Construktion();

//will be some variation of the following
//String-1234
var result = construktion.Construct<string>();

//75
var result = construktion.Construct<int>();

//true or false
var result = construktion.Construct<bool>();
```

Most built in types are supported by default. Classes would be constructed the same way.

```c#
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

var person = construktion.Construct<Person>();

person.Name.ShouldStartWith("Name-");
person.Age.ShouldNotBe(0);
```

Nested classes and Enumerables will also be constructed

```c#
public class Order
{
    public Guid Id { get; set; }
    public Customer Customer { get; set; }
    public IEnumerable<Product> Products { get; set; }
}

public class Customer
{
    public string Name { get; set; }
}

public class Product
{
    public string Sku { get; set; }
}

var order = construktion.Construct<Order>();

order.Id.ShouldNotBe(Guid.Empty);
order.Customer.Name.ShouldNotBeNullOrWhiteSpace();
order.Products.Count().ShouldBe(3);
order.Products.ShouldAllBe(x => !string.IsNullOrWhiteSpace(x.Sku));
```
Nuget Package
---
This [package](https://www.nuget.org/packages/Construktion) is currently in pre release and looking for early adopters/contributors.
```
Install-Package Construktion -Pre
```
Customizing
---
Check out the [wiki](https://github.com/Construktion/Construktion/wiki) for more details about the library and how it can be customized. 

Owner
---
Joe Gannon, joegannon15@gmail.com