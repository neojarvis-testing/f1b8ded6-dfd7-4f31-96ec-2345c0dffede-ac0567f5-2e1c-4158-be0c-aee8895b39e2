using dotnetapp.Exceptions;
using dotnetapp.Models;
using dotnetapp.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using System.Reflection;
using dotnetapp.Services;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace dotnetapp.Tests
{
    [TestFixture]
    public class Tests
    {
        private ApplicationDbContext _context; 
        private HttpClient _httpClient;

        [SetUp]
       public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            _context = new ApplicationDbContext(options);
           
             _httpClient = new HttpClient();
             _httpClient.BaseAddress = new Uri("http://localhost:8080");

        }

        [TearDown]
        public void TearDown()
        {
             _context.Dispose();
        }

   [Test, Order(1)]
    public async Task Backend_Test_Post_Method_Register_Admin_Returns_HttpStatusCode_OK()
    {
        ClearDatabase();
        string uniqueId = Guid.NewGuid().ToString();

        // Generate a unique userName based on a timestamp
        string uniqueUsername = $"abcd_{uniqueId}";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";

        string requestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"abc@123A\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\", \"UserRole\": \"Admin\"}}";
        HttpResponseMessage response = await _httpClient.PostAsync("/api/register", new StringContent(requestBody, Encoding.UTF8, "application/json"));

        Console.WriteLine(response.StatusCode);
        string responseString = await response.Content.ReadAsStringAsync();

        Console.WriteLine(responseString);
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
  
   [Test, Order(2)]
    public async Task Backend_Test_Post_Method_Login_Admin_Returns_HttpStatusCode_OK()
    {
        ClearDatabase();

        string uniqueId = Guid.NewGuid().ToString();

        // Generate a unique userName based on a timestamp
        string uniqueUsername = $"abcd_{uniqueId}";
        string uniqueEmail = $"abcd{uniqueId}@gmail.com";

        string requestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"abc@123A\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\", \"UserRole\": \"Admin\"}}";
        HttpResponseMessage response = await _httpClient.PostAsync("/api/register", new StringContent(requestBody, Encoding.UTF8, "application/json"));

        // Print registration response
        string registerResponseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Registration Response: " + registerResponseBody);

        // Login with the registered user
        string loginRequestBody = $"{{\"Email\" : \"{uniqueEmail}\",\"Password\" : \"abc@123A\"}}"; // Updated variable names
        HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));

        // Print login response
        string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
        Console.WriteLine("Login Response: " + loginResponseBody);

        Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
    }
   

   [Test, Order(3)]
    public async Task Backend_Test_Get_All_BlogPosts_With_Token_By_Admin_Returns_HttpStatusCode_OK()
    {
    ClearDatabase();
    string uniqueId = Guid.NewGuid().ToString();

    // Generate a unique userName based on a timestamp
    string uniqueUsername = $"abcd_{uniqueId}";
    string uniqueEmail = $"abcd{uniqueId}@gmail.com";

    string requestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"abc@123A\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\", \"UserRole\": \"Admin\"}}";
    HttpResponseMessage response = await _httpClient.PostAsync("/api/register", new StringContent(requestBody, Encoding.UTF8, "application/json"));

    // Print registration response
    string registerResponseBody = await response.Content.ReadAsStringAsync();
    Console.WriteLine("Registration Response: " + registerResponseBody);

    // Login with the registered user
    string loginRequestBody = $"{{\"Email\" : \"{uniqueEmail}\",\"Password\" : \"abc@123A\"}}"; // Updated variable names
    HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));

    // Print login response
    string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
    Console.WriteLine("Login Response: " + loginResponseBody);

    Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
    string responseBody = await loginResponse.Content.ReadAsStringAsync();

    dynamic responseMap = JsonConvert.DeserializeObject(responseBody);

    string token = responseMap.token;

    Assert.IsNotNull(token);

    // Use the token to get all feeds
    _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
    HttpResponseMessage apiResponse = await _httpClient.GetAsync("/api/blogposts");

    // Print feed response
    string apiResponseBody = await apiResponse.Content.ReadAsStringAsync();
    Console.WriteLine("apiResponseBody: " + apiResponseBody);

    Assert.AreEqual(HttpStatusCode.OK, apiResponse.StatusCode);
}

[Test, Order(4)]
public async Task Backend_Test_Get_AllBlogPosts_Without_Token_By_Admin_Returns_HttpStatusCode_Unauthorized()
{
    ClearDatabase();
    string uniqueId = Guid.NewGuid().ToString();

    // Generate a unique userName based on a timestamp
    string uniqueUsername = $"abcd_{uniqueId}";
    string uniqueEmail = $"abcd{uniqueId}@gmail.com";

    string requestBody = $"{{\"Username\": \"{uniqueUsername}\", \"Password\": \"abc@123A\", \"Email\": \"{uniqueEmail}\", \"MobileNumber\": \"1234567890\", \"UserRole\": \"Admin\"}}";
    HttpResponseMessage response = await _httpClient.PostAsync("/api/register", new StringContent(requestBody, Encoding.UTF8, "application/json"));

    // Print registration response
    string registerResponseBody = await response.Content.ReadAsStringAsync();
    Console.WriteLine("Registration Response: " + registerResponseBody);

    // Login with the registered user
    string loginRequestBody = $"{{\"Email\" : \"{uniqueEmail}\",\"Password\" : \"abc@123A\"}}"; // Updated variable names
    HttpResponseMessage loginResponse = await _httpClient.PostAsync("/api/login", new StringContent(loginRequestBody, Encoding.UTF8, "application/json"));

    // Print login response
    string loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
    Console.WriteLine("Login Response: " + loginResponseBody);

    Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);
    string responseBody = await loginResponse.Content.ReadAsStringAsync();

    HttpResponseMessage apiResponse = await _httpClient.GetAsync("/api/blogposts");

    // Print feed response
    string apiResponseBody = await apiResponse.Content.ReadAsStringAsync();
    Console.WriteLine("apiResponseBody: " + apiResponseBody);

    Assert.AreEqual(HttpStatusCode.Unauthorized, apiResponse.StatusCode);
}

     
[Test, Order(5)]
 public async Task Backend_Test_Post_Method_Add_BlogPost_In_BlogPostService_Adds_BlogPost_Successfully()
        {
            ClearDatabase();

            // Set up user data
          var userData = new Dictionary<string, object>
        {
        { "UserId", 1 },
        { "Username", "testuser" },
        { "Password", "testpassword" },
        { "Email", "test@example.com" },
        { "MobileNumber", "1234567890" },
        { "UserRole", "User" }
        };

            // Create user instance and set properties
            var user = new User();
            foreach (var kvp in userData)
            {
                var propertyInfo = typeof(User).GetProperty(kvp.Key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(user, kvp.Value);
                }
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Set up blog post data
            var blogPostData = new Dictionary<string, object>
            {
                { "BlogPostId", 1 },
                { "UserId", 1 },
                { "Title", "First Blog Post" },
                { "Content", "This is the content of the first blog post." },
                { "Status", "Published" },
                { "PublishedDate", DateTime.Now }
            };

            // Create blog post instance and set properties
            var blogPost = new BlogPost();
            foreach (var kvp in blogPostData)
            {
                var propertyInfo = typeof(BlogPost).GetProperty(kvp.Key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(blogPost, kvp.Value);
                }
            }

            // Load assembly and types
            string assemblyName = "dotnetapp";
            Assembly assembly = Assembly.Load(assemblyName);
            string serviceName = "dotnetapp.Services.BlogPostService";
            Type serviceType = assembly.GetType(serviceName);

            // Get the AddBlogPost method
            MethodInfo addBlogPostMethod = serviceType.GetMethod("AddBlogPost");

            // Check if method exists
            if (addBlogPostMethod != null)
            {
                var service = Activator.CreateInstance(serviceType, _context);
                var addBlogPostTask = (Task<bool>)addBlogPostMethod.Invoke(service, new object[] { blogPost });
                await addBlogPostTask;

                // Verify that the blog post was added
                var retrievedBlogPost = await _context.BlogPosts.FindAsync(1);
                Assert.IsNotNull(retrievedBlogPost);
                Assert.AreEqual(blogPost.Title, retrievedBlogPost.Title);
            }
            else
            {
                Assert.Fail();
            }
        }

[Test, Order(6)]
public async Task Backend_Test_Post_Method_Add_BlogPost_In_BlogPostService_Throws_BlogPostException_If_BlogPostTitle_Already_Exists()
{
    ClearDatabase();

    // Set up user data
    var userData = new Dictionary<string, object>
    {
        { "UserId", 1 },
        { "Username", "testuser" },
        { "Password", "testpassword" },
        { "Email", "test@example.com" },
        { "MobileNumber", "1234567890" },
        { "UserRole", "User" }
    };

    // Create user instance and set properties
    var user = new User();
    foreach (var kvp in userData)
    {
        var propertyInfo = typeof(User).GetProperty(kvp.Key);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(user, kvp.Value);
        }
    }

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    // Set up initial blog post data
    var blogPostData = new Dictionary<string, object>
    {
        { "BlogPostId", 1 },
        { "UserId", 1 },
        { "Title", "First Blog Post" },
        { "Content", "This is the content of the first blog post." },
        { "Status", "Published" },
        { "PublishedDate", DateTime.Now }
    };

    var blogPost = new BlogPost();
    foreach (var kvp in blogPostData)
    {
        var propertyInfo = typeof(BlogPost).GetProperty(kvp.Key);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(blogPost, kvp.Value);
        }
    }

    _context.BlogPosts.Add(blogPost);
    await _context.SaveChangesAsync();

    // Load assembly and types
    string assemblyName = "dotnetapp";
    Assembly assembly = Assembly.Load(assemblyName);
    string serviceName = "dotnetapp.Services.BlogPostService";
    Type serviceType = assembly.GetType(serviceName);

    // Get the AddBlogPost method
    MethodInfo addBlogPostMethod = serviceType.GetMethod("AddBlogPost");

    // Check if method exists
    if (addBlogPostMethod != null)
    {
        var service = Activator.CreateInstance(serviceType, _context);

        // Attempt to add a new blog post with the same title
        var newBlogPostData = new Dictionary<string, object>
        {
            { "BlogPostId", 2 },
            { "UserId", 1 },
            { "Title", "First Blog Post" },
            { "Content", "This is the content of the second blog post." },
            { "Status", "Draft" },
            { "PublishedDate", DateTime.Now }
        };

        var newBlogPost = new BlogPost();
        foreach (var kvp in newBlogPostData)
        {
            var propertyInfo = typeof(BlogPost).GetProperty(kvp.Key);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(newBlogPost, kvp.Value);
            }
        }

        try
        {
            var addBlogPostTask = (Task<bool>)addBlogPostMethod.Invoke(service, new object[] { newBlogPost });
            // Await the task to ensure exception is caught
            Console.WriteLine("res" + addBlogPostTask.Result);
            // If no exception is thrown, fail the test
            Assert.Fail();
        }
        catch (Exception ex)
        {
            Assert.IsNotNull(ex.InnerException);
            Assert.IsTrue(ex.InnerException is BlogPostException);
            Assert.AreEqual("A blog post with the same title already exists", ex.InnerException.Message);
        }
    }
    else
    {
        Assert.Fail("Method AddBlogPost not found in BlogPostService");
    }
}

[Test, Order(7)]
public async Task Backend_Test_Get_Method_Get_BlogPost_By_Id_In_BlogPostService_Fetches_BlogPost_Successfully()
        {
            ClearDatabase();

            // Set up user data
             var userData = new Dictionary<string, object>
        {
        { "UserId", 1 },
        { "Username", "testuser" },
        { "Password", "testpassword" },
        { "Email", "test@example.com" },
        { "MobileNumber", "1234567890" },
        { "UserRole", "User" }
        };

            // Create user instance and set properties
            var user = new User();
            foreach (var kvp in userData)
            {
                var propertyInfo = typeof(User).GetProperty(kvp.Key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(user, kvp.Value);
                }
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Set up blog post data
            var blogPostData = new Dictionary<string, object>
            {
                { "BlogPostId", 1 },
                { "UserId", 1 },
                { "Title", "First Blog Post" },
                { "Content", "This is the content of the first blog post." },
                { "Status", "Published" },
                { "PublishedDate", DateTime.Now }
            };

            // Create blog post instance and set properties
            var blogPost = new BlogPost();
            foreach (var kvp in blogPostData)
            {
                var propertyInfo = typeof(BlogPost).GetProperty(kvp.Key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(blogPost, kvp.Value);
                }
            }

            _context.BlogPosts.Add(blogPost);
            await _context.SaveChangesAsync();

            // Load assembly and types
            string assemblyName = "dotnetapp";
            Assembly assembly = Assembly.Load(assemblyName);
            string serviceName = "dotnetapp.Services.BlogPostService";
            Type serviceType = assembly.GetType(serviceName);

            // Get the GetBlogPostById method
            MethodInfo getBlogPostByIdMethod = serviceType.GetMethod("GetBlogPostById");

            // Check if method exists
            if (getBlogPostByIdMethod != null)
            {
                var service = Activator.CreateInstance(serviceType, _context);
                var retrievedBlogPostTask = (Task<BlogPost>)getBlogPostByIdMethod.Invoke(service, new object[] { 1 });
                var retrievedBlogPost = await retrievedBlogPostTask;

                // Assert the retrieved blog post is not null and properties match
                Assert.IsNotNull(retrievedBlogPost);
                Assert.AreEqual(blogPost.Title, retrievedBlogPost.Title);
            }
            else
            {
                Assert.Fail();
            }
        }

[Test, Order(8)]
public async Task Backend_Test_GetAll_Method_Get_All_BlogPosts_In_BlogPostService_Fetches_All_BlogPosts_Successfully()
        {
            ClearDatabase();

            // Set up user data
              var userData = new Dictionary<string, object>
        {
        { "UserId", 1 },
        { "Username", "testuser" },
        { "Password", "testpassword" },
        { "Email", "test@example.com" },
        { "MobileNumber", "1234567890" },
        { "UserRole", "User" }
        };

            // Create user instance and set properties
            var user = new User();
            foreach (var kvp in userData)
            {
                var propertyInfo = typeof(User).GetProperty(kvp.Key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(user, kvp.Value);
                }
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Set up blog post data
            var blogPostData1 = new Dictionary<string, object>
            {
                { "BlogPostId", 1 },
                { "UserId", user.UserId },
                { "Title", "First Blog Post" },
                { "Content", "This is the content of the first blog post." },
                { "Status", "Published" },
                { "PublishedDate", DateTime.Now }
            };

            var blogPostData2 = new Dictionary<string, object>
            {
                { "BlogPostId", 2 },
                { "UserId", user.UserId },
                { "Title", "Second Blog Post" },
                { "Content", "This is the content of the second blog post." },
                { "Status", "Draft" },
                { "PublishedDate", DateTime.Now }
            };

            // Create blog post instances and set properties
            var blogPost1 = new BlogPost();
            foreach (var kvp in blogPostData1)
            {
                var propertyInfo = typeof(BlogPost).GetProperty(kvp.Key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(blogPost1, kvp.Value);
                }
            }

            var blogPost2 = new BlogPost();
            foreach (var kvp in blogPostData2)
            {
                var propertyInfo = typeof(BlogPost).GetProperty(kvp.Key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(blogPost2, kvp.Value);
                }
            }

            _context.BlogPosts.Add(blogPost1);
            _context.BlogPosts.Add(blogPost2);
            await _context.SaveChangesAsync();

            // Load assembly and types
            string assemblyName = "dotnetapp";
            Assembly assembly = Assembly.Load(assemblyName);
            string serviceName = "dotnetapp.Services.BlogPostService";
            Type serviceType = assembly.GetType(serviceName);

            // Get the GetAllBlogPosts method
            MethodInfo getAllBlogPostsMethod = serviceType.GetMethod("GetAllBlogPosts");

            // Check if method exists
            if (getAllBlogPostsMethod != null)
            {
                var service = Activator.CreateInstance(serviceType, _context);
                var retrievedBlogPostsTask = (Task<IEnumerable<BlogPost>>)getAllBlogPostsMethod.Invoke(service, null);
                var retrievedBlogPosts = await retrievedBlogPostsTask;

                // Assert the retrieved blog posts are not null and match the expected count
                Assert.IsNotNull(retrievedBlogPosts);
                Assert.AreEqual(2, retrievedBlogPosts.Count());
            }
            else
            {
                Assert.Fail();
            }
        }

[Test, Order(9)]
public async Task Backend_Test_Delete_Method_Delete_BlogPost_By_Id_In_BlogPostService_Deletes_BlogPost_Successfully()
        {
            ClearDatabase();

            // Set up user data
              var userData = new Dictionary<string, object>
        {
        { "UserId", 1 },
        { "Username", "testuser" },
        { "Password", "testpassword" },
        { "Email", "test@example.com" },
        { "MobileNumber", "1234567890" },
        { "UserRole", "User" }
        };

            // Create user instance and set properties
            var user = new User();
            foreach (var kvp in userData)
            {
                var propertyInfo = typeof(User).GetProperty(kvp.Key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(user, kvp.Value);
                }
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Set up blog post data
            var blogPostData = new Dictionary<string, object>
            {
                { "BlogPostId", 1 },
                { "UserId", user.UserId },
                { "Title", "First Blog Post" },
                { "Content", "This is the content of the first blog post." },
                { "Status", "Published" },
                { "PublishedDate", DateTime.Now }
            };

            // Create blog post instance and set properties
            var blogPost = new BlogPost();
            foreach (var kvp in blogPostData)
            {
                var propertyInfo = typeof(BlogPost).GetProperty(kvp.Key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(blogPost, kvp.Value);
                }
            }

            _context.BlogPosts.Add(blogPost);
            await _context.SaveChangesAsync();

            // Load assembly and types
            string assemblyName = "dotnetapp";
            Assembly assembly = Assembly.Load(assemblyName);
            string serviceName = "dotnetapp.Services.BlogPostService";
            Type serviceType = assembly.GetType(serviceName);

            // Get the DeleteBlogPost method
            MethodInfo deleteBlogPostMethod = serviceType.GetMethod("DeleteBlogPost");

            // Check if method exists
            if (deleteBlogPostMethod != null)
            {
                var service = Activator.CreateInstance(serviceType, _context);
                var deleteBlogPostTask = (Task<bool>)deleteBlogPostMethod.Invoke(service, new object[] { 1 });
                var result = await deleteBlogPostTask;

                // Assert the blog post was deleted successfully
                Assert.IsTrue(result);

                // Verify that the blog post no longer exists
                var retrievedBlogPost = await _context.BlogPosts.FindAsync(1);
                Assert.IsNull(retrievedBlogPost);
            }
            else
            {
                Assert.Fail();
            }
        }


[Test, Order(10)]
public async Task Backend_Test_GetAll_Method_Get_All_Announcements_In_Announcement_Service_Fetches_All_Announcements_Successfully()
        {
            ClearDatabase();

            // Set up announcement data
            var announcementData1 = new Dictionary<string, object>
            {
                { "AnnouncementId", 1 },
                { "Title", "First Announcement" },
                { "Content", "This is the content of the first announcement." },
                { "PublishedDate", DateTime.Now },
                { "Category", "General" },
                { "Priority", "High" },
                { "Status", "Active" }
            };

            var announcementData2 = new Dictionary<string, object>
            {
                { "AnnouncementId", 2 },
                { "Title", "Second Announcement" },
                { "Content", "This is the content of the second announcement." },
                { "PublishedDate", DateTime.Now },
                { "Category", "HR" },
                { "Priority", "Low" },
                { "Status", "Inactive" }
            };

            // Create announcement instances and set properties
            var announcement1 = new Announcement();
            foreach (var kvp in announcementData1)
            {
                var propertyInfo = typeof(Announcement).GetProperty(kvp.Key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(announcement1, kvp.Value);
                }
            }

            var announcement2 = new Announcement();
            foreach (var kvp in announcementData2)
            {
                var propertyInfo = typeof(Announcement).GetProperty(kvp.Key);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(announcement2, kvp.Value);
                }
            }

            _context.Announcements.Add(announcement1);
            _context.Announcements.Add(announcement2);
            await _context.SaveChangesAsync();

            // Load assembly and types
            string assemblyName = "dotnetapp";
            Assembly assembly = Assembly.Load(assemblyName);
            string serviceName = "dotnetapp.Services.AnnouncementService";
            Type serviceType = assembly.GetType(serviceName);

            // Get the GetAllAnnouncements method
            MethodInfo getAllAnnouncementsMethod = serviceType.GetMethod("GetAllAnnouncements");

            // Check if method exists
            if (getAllAnnouncementsMethod != null)
            {
                var service = Activator.CreateInstance(serviceType, _context);
                var retrievedAnnouncementsTask = (Task<IEnumerable<Announcement>>)getAllAnnouncementsMethod.Invoke(service, null);
                var retrievedAnnouncements = await retrievedAnnouncementsTask;

                // Assert the retrieved announcements are not null and match the expected count
                Assert.IsNotNull(retrievedAnnouncements);
                Assert.AreEqual(2, retrievedAnnouncements.Count());
            }
            else
            {
                Assert.Fail();
            }
        }


[Test, Order(11)]
public async Task Backend_Test_Post_Method_AddFeedback_In_Feedback_Service_Posts_Successfully()
{
        ClearDatabase();

    // Add user
    var userData = new Dictionary<string, object>
    {
        { "UserId",42 },
        { "Username", "testuser" },
        { "Password", "testpassword" },
        { "Email", "test@example.com" },
        { "MobileNumber", "1234567890" },
        { "UserRole", "User" }
    };

    var user = new User();
    foreach (var kvp in userData)
    {
        var propertyInfo = typeof(User).GetProperty(kvp.Key);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(user, kvp.Value);
        }
    }
    _context.Users.Add(user);
    _context.SaveChanges();
    // Add loan application
    string assemblyName = "dotnetapp";
    Assembly assembly = Assembly.Load(assemblyName);
    string ServiceName = "dotnetapp.Services.FeedbackService";
    string typeName = "dotnetapp.Models.Feedback";

    Type serviceType = assembly.GetType(ServiceName);
    Type modelType = assembly.GetType(typeName);

    MethodInfo method = serviceType.GetMethod("AddFeedback", new[] { modelType });

    if (method != null)
    {
           var feedbackData = new Dictionary<string, object>
            {
                { "FeedbackId", 11 },
                { "UserId", 42 },
                { "FeedbackText", "Great experience!" },
                { "Date", DateTime.Now }
            };
        var feedback = new Feedback();
        foreach (var kvp in feedbackData)
        {
            var propertyInfo = typeof(Feedback).GetProperty(kvp.Key);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(feedback, kvp.Value);
            }
        }
        var service = Activator.CreateInstance(serviceType, _context);
        var result = (Task<bool>)method.Invoke(service, new object[] { feedback });
    
        var addedFeedback= await _context.Feedbacks.FindAsync(11);
        Assert.IsNotNull(addedFeedback);
        Assert.AreEqual("Great experience!",addedFeedback.FeedbackText);

    }
    else{
        Assert.Fail();
    }
}

[Test, Order(12)]
public async Task Backend_Test_Delete_Method_Feedback_In_Feeback_Service_Deletes_Successfully()
{
    // Add user
     ClearDatabase();

    var userData = new Dictionary<string, object>
    {
        { "UserId",42 },
        { "Username", "testuser" },
        { "Password", "testpassword" },
        { "Email", "test@example.com" },
        { "MobileNumber", "1234567890" },
        { "UserRole", "User" }
    };

    var user = new User();
    foreach (var kvp in userData)
    {
        var propertyInfo = typeof(User).GetProperty(kvp.Key);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(user, kvp.Value);
        }
    }
    _context.Users.Add(user);
    _context.SaveChanges();

           var feedbackData = new Dictionary<string, object>
            {
                { "FeedbackId", 11 },
                { "UserId", 42 },
                { "FeedbackText", "Great experience!" },
                { "Date", DateTime.Now }
            };
        var feedback = new Feedback();
        foreach (var kvp in feedbackData)
        {
            var propertyInfo = typeof(Feedback).GetProperty(kvp.Key);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(feedback, kvp.Value);
            }
        }
     _context.Feedbacks.Add(feedback);
    _context.SaveChanges();
    // Add loan application
    string assemblyName = "dotnetapp";
    Assembly assembly = Assembly.Load(assemblyName);
    string ServiceName = "dotnetapp.Services.FeedbackService";
    string typeName = "dotnetapp.Models.Feedback";

    Type serviceType = assembly.GetType(ServiceName);
    Type modelType = assembly.GetType(typeName);

  
    MethodInfo deletemethod = serviceType.GetMethod("DeleteFeedback", new[] { typeof(int) });

    if (deletemethod != null)
    {
        var service = Activator.CreateInstance(serviceType, _context);
        var deleteResult = (Task<bool>)deletemethod.Invoke(service, new object[] { 11 });

        var deletedFeedbackFromDb = await _context.Feedbacks.FindAsync(11);
        Assert.IsNull(deletedFeedbackFromDb);
    }
    else
    {
        Assert.Fail();
    }
}

[Test, Order(13)]
public async Task Backend_Test_Get_Method_GetFeedbacksByUserId_In_Feedback_Service_Fetches_Successfully()
{
    ClearDatabase();

    // Add user
    var userData = new Dictionary<string, object>
    {
        { "UserId", 330 },
        { "Username", "testuser" },
        { "Password", "testpassword" },
        { "Email", "test@example.com" },
        { "MobileNumber", "1234567890" },
        { "UserRole", "User" }
    };

    var user = new User();
    foreach (var kvp in userData)
    {
        var propertyInfo = typeof(User).GetProperty(kvp.Key);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(user, kvp.Value);
        }
    }
    _context.Users.Add(user);
    _context.SaveChanges();

    var feedbackData= new Dictionary<string, object>
    {
        { "FeedbackId", 13 },
        { "UserId", 330 },
        { "FeedbackText", "Great experience!" },
        { "Date", DateTime.Now }
    };

    var feedback = new Feedback();
    foreach (var kvp in feedbackData)
    {
        var propertyInfo = typeof(Feedback).GetProperty(kvp.Key);
        if (propertyInfo != null)
        {
            propertyInfo.SetValue(feedback, kvp.Value);
        }
    }
    _context.Feedbacks.Add(feedback);
    _context.SaveChanges();

    // Add loan application
    string assemblyName = "dotnetapp";
    Assembly assembly = Assembly.Load(assemblyName);
    string ServiceName = "dotnetapp.Services.FeedbackService";
    string typeName = "dotnetapp.Models.Feedback";

    Type serviceType = assembly.GetType(ServiceName);
    Type modelType = assembly.GetType(typeName);

    MethodInfo method = serviceType.GetMethod("GetFeedbacksByUserId");

    if (method != null)
    {
        var service = Activator.CreateInstance(serviceType, _context);
        var result = ( Task<IEnumerable<Feedback>>)method.Invoke(service, new object[] {330});
        Assert.IsNotNull(result);
         var check=true;
        foreach (var item in result.Result)
        {
            check=false;
            Assert.AreEqual("Great experience!", item.FeedbackText);
   
        }
        if(check==true)
        {
            Assert.Fail();

        }
    }
    else{
        Assert.Fail();
    }
}
    private void ClearDatabase()
    {
    _context.Database.EnsureDeleted();
    _context.Database.EnsureCreated();
    }

    }
}

