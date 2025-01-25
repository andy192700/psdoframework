
# Module demonstrating Do's ability to also support functions via modules as well as classes.

# One function that simply doubles a number.
function DoubleANumber {
    param (
        [int] $number
    )

    return $number * 2;
}

# Another function that throws an exception.
function ThrowsAnException {
    throw "Exception thrown!!!";
}

Export-ModuleMember -Function DoubleANumber;
Export-ModuleMember -Function ThrowsAnException;