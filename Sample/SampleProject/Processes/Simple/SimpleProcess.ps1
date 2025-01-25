# Example Process that simply echos hello world with no dependency injection
class SimpleProcess : DoFramework.Processing.Process {
    [void] Run() {
        Write-Host "hello world!";
    }
}
