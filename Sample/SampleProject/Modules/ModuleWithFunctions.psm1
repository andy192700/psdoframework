function DoubleANumber {
    param (
        [int] $number
    )

    return $number * 2;
}

function ThrowsAnException {
    throw "Exception thrown!!!";
}

Export-ModuleMember -Function DoubleANumber;
Export-ModuleMember -Function ThrowsAnException;