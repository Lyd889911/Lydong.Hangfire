# 介绍
封装了hangfire，更简单的执行定时任务。
# 配置
```csharp
builder.Services.AddLydongHangfire();
```
如果要使用面板
```csharp
app.UseLydongHangfire();
```
两个方法都可以传递一个`LydongHangfireOption`参数配置。使用面板的时候记得配置`LoginUserName`和`LoginPassword`。
包含了三种存储方式。默认`Memory`
```csharp
    public enum HangfireStorageType
    {
        SqlServer,
        Redis,
        Memory
    }
```
# 使用
实现`IJob`接口的类，会自动注入到IOC容器。
该类里面的方法满足以下条件的就会被注册定时任务：

1. `public`
2. 没有参数
3. 方法上标记特性`[Job]`，特性必须写Cron
```csharp
public class TestJob:IJob
{
    [Job(Cron = "*/2 * * * * *")]
    public void T1()
    {
        Console.WriteLine($"t1:{DateTime.Now}");
    }

    [Job(Cron = "*/7 * * * * *")]
    public void T2()
    {
        Console.WriteLine($"t2:{DateTime.Now}");
    }

    [Job(Cron = "*/17 * * * * *")]
    public void T3()
    {
        Console.WriteLine($"t3:{DateTime.Now}");
    }
}
```
