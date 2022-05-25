<div align="center">
<h1>Basic Calculator</h1>
<p align="center">Straight-forward, responsive and easy-to-use CLI Calculator.</p>
</div>

# Synopsis
A .NET Calculator Console Application written in C#. The very first application project I've personally written and published on GitHub. As the repository's title suggests it's a calculator capable of evaluating basic arithmetic infix expressions by typing or passing your infix expression for evaluation, rather than clicking GUI buttons on a digital calculator.

# Features
* **Fast Performance**: Calculate away with a simplistic interface!
* **Basic Operators**: Addition (+), subtraction (-), multiplication (*), decimal division (/), modulo (%) and power of number (^).  
* **Supports Parentheses**: For encapsulating infix expressions (2(2+5)).
* **Infix to Postfix conversion**: Implements a traditional evaluation process, similar in most reverse polish notation implemented calculators. And **automatically prints the created postfix tokens** alongside your results.

# Building
1. Install .NET SDK (check project's target framework for correct SDK)
2. Clone "BasicCalculator" repository & switch directory.
3. Compiling publish-ready builds via .NET CLI as it's cross-platform and accessible. Alternatively you can use MSBuild if you so choose separately from this guide.
### dotnet publish -c Release -r win-x64 --self-contained true
4. Run the following command above. Change it's runtime ID for Linux, MacOS or other targeting platforms, such as, **linux-x64** or **osx-x64** .etc., Check out [.NET RID Catalog.](https://docs.microsoft.com/en-gb/dotnet/core/rid-catalog)
### BasicCalculator\Calculator\bin\Release\\<TARGET_FRAMEWORK>\win-x64
5. Build should be compiled in the following directory by default.

# Testing
Apart from manual trial and error, NUnit Framework provides testing units - an open source unit-testing framework for all .NET Languages. It's licensed under MIT, available on [GitHub](https://github.com/nunit/nunit). Highly recommended for test driven development and for testing all of your components/functions.
### dotnet test
Unit testing via .NET CLI invoking "dotnet test" command, executes NUnit's Test Runner.

# Contributing
Clone, forking/submitting pull requests etc., are welcome. When submitting pull requests keep in mind the following - Consistent coding style, keep it simple (features etc.,) and most importantly, why the proposed benefits/changes? All your contributions are licensed under [GNU General Public License v3.0](https://github.com/SerioDmGuy/BasicCalculator/blob/master/LICENSE). If it's a concern be sure to mention within your pull request.

# License
Licensed as [GNU General Public License v3.0](https://github.com/SerioDmGuy/BasicCalculator/blob/master/LICENSE) a strong copyleft free open-source license. Protected by Free Software Foundation.