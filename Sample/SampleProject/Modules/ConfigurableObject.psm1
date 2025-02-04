class MyConfig {
    [string] $MyString;
    [int] $MyInt;
}

class MySecondConfig {
    [double] $MyDouble;
    [bool] $MyBool;
}

class MyThirdConfig {
    [short] $MyShort;
    [float] $MyFloat;
}

class MyConfigurationService {
    [MyConfig] $Config1;
    [MySecondConfig] $Config2;
    [MyThirdConfig] $Config3;

    MyConfigurationService(
        [MyConfig] $config1,
        [MySecondConfig] $config2,
        [MyThirdConfig] $config3) {
        $this.Config1 = $config1;
        $this.Config2 = $config2;
        $this.Config3 = $config3;
    }
}