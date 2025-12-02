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
[int]$dialValue = 50

foreach ($turn in $challenge.Split("`n")) {
    [int]$v = $turn.Substring(1)
    $distance = if ($turn[0] -eq 'L') { -$v } else { $v }

    $start = $dialValue
    $end   = $dialValue + $distance

    $dialValue = (($end % 100) + 100) % 100

    # Count every time we hit 0 along the way
    for ($i = 1; $i -le [math]::Abs($distance); $i++) {
        $step = ($start + ($distance -lt 0 ? -$i : $i)) % 100
        if ($step -lt 0) { $step += 100 }
        if ($step -eq 0) { $zero_count++ }
    }

    Write-Host "Turn $turn -> dial $dialValue, zero_count $zero_count"
}

Write-Host "Final Count of Zeros is $zero_count"

