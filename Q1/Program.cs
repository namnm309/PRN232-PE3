    using Microsoft.EntityFrameworkCore;
    using Q1.Data;

var builder = WebApplication.CreateBuilder(args);

//Thêm DbContext éo thêm khỏi chơi
    builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

builder.Services
    .AddControllers()//Ko add khỏi chơi controller 
    .AddXmlSerializerFormatters();//Này là định dạng cho reponse ra xml NẾU ĐỀ CÓ YÊU CẦU \\ mặc định là json của .net nếu đề yêu cầu thêm xml thì thêm dòng này

//test swagger thêm 2 này
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

var app = builder.Build();// dòng này phải nằm cuối cùng của mấy cái build ko thì khỏi build

//app.MapGet("/", () => "Hello World!");                => này trường cho bỏ đi thay bằng cái dưới để ánh xạ mấy cái endpoint



//Thêm này nếu muốn chơi swagger , nma test = postman đi cho chắc tại mở chrome lên sợ dính bả
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription(); ;

//====================================================
app.MapControllers();

app.Run();
