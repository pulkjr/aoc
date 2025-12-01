$challenge = @'
L68
L30
R48
L5
R60
L55
L1
L99
R114
L182
'@

function subtract
{
    param(
        $currentDial,
        [int]$turnAmount
    )

    $tempValue = $currentDial - $turnAmount

    while ( $tempValue -lt 0)
    {
        $tempValue = 100 + $tempValue
    }
    return $tempValue
}
function add
{
    param(
        $currentDial,
        [int]$turnAmount
    )

    $tempValue = $currentDial + $turnAmount

    while ( $tempValue -gt 99)
    {
        $tempValue = $tempValue - 100
    }
    return $tempValue
}

$zero_count = 0
$dialValue = 50

foreach($turn in $challenge.Split("`n"))
{
    Write-Host "Turn: $turn"
    if($turn.StartsWith('L'))
    {
        $dialValue = subtract -currentDial $dialValue -turnAmount $turn.TrimStart('L')
    } else
    {
        $dialValue = add -currentDial $dialValue -turnAmount $turn.TrimStart('R')
    }
    Write-Host " - dialValue: $dialValue"
    if ( $dialValue -eq 0)
    {
        Write-Host ' - YES a Zero'
        $zero_count++
    }
}

Write-Host "Final Count of Zeros is $zero_count"

