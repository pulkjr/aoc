[string]$rawInput = Get-Content -Path './input.csv' -Raw

[decimal]$finalValue = 0
foreach($inputRange in $rawInput.Replace('\n', '').Split(',', [System.StringSplitOptions]::RemoveEmptyEntries))
{
    [string]$inputRange = $inputRange.Trim()

    if ([string]::IsNullOrWhiteSpace($inputRange))
    {
        continue
    }
    [decimal]$start, [decimal]$finish = $inputRange.Split('-',[System.StringSplitOptions]::TrimEntries)

    :numberLoop for($i = $start; $i -le $finish; $i++)
    {
        $itterString = "$i"
        if ( $itterString[0] -eq '0')
        {
            continue
        }
        if($itterString.Length % 2 -ne 0)
        {
            continue
        }
        [decimal]$length = $itterString.Length
        [decimal]$half = $length / 2
        # Write-Host "Number: $itterString"
        # Write-Host "Length: $length"
        # Write-Host "Half: $half"

        for ( $charI = 0; $charI -lt $half; $charI++)
        {
            # Write-Host "$charI : $($itterString[$charI]) -ne $($charI + $half): $($itterString[($charI + $half)])"
            if($itterString[$charI] -ne $itterString[($charI + $half)])
            {
                continue numberLoop
            }
        }
        # Write-Host -ForegroundColor green "$i is a valid number"
        $finalValue += $i
    }

}
Write-Host $finalValue
