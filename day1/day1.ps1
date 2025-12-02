$challenge = @'
L68
L30
R48
L5
R60
L55
L1
L99
R14
L82
'@

$zero_count = 0
$dialValue = 50

foreach($turn in $challenge.Split("`n"))
{
    $start = $dialValue
    Write-Host "Start Dial: $dialValue  Turn: $turn"
    [int]$v = $turn.Substring(1);

    if ( $turn[0] -eq 'L')
    {
        $dialValue -= $v
    } else
    {
        $dialValue += $v
    }

    $r = 0
    if ( $dialValue -ne 100 )
    {
        $r = [math]::Abs([math]::Floor($dialValue / 100))
        Write-Host " - r: $r"
    }
    if( $start -eq 0 -and $r -gt 0)
    {
        $r--
    }

    $dialValue = $dialValue % 100

    if($dialValue -lt 0)
    {
        $dialValue += 100
    }
    if ( $dialValue -eq 0)
    {
        $zero_count++;
    }

    $zero_count += $r

    Write-Host " - dialValue: $dialValue, zero_count: $zero_count"
}

Write-Host "Final Count of Zeros is $zero_count"

