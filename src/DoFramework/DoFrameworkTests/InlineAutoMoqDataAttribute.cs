using AutoFixture.Xunit2;

namespace DoFrameworkTests;

public class InlineAutoMoqDataAttribute(params object[] objects) : InlineAutoDataAttribute(new AutoDataAttribute(), objects) { }