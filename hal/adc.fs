\ Слой абстракции для работы с АЦП.

-hal-adc
marker -hal-adc

\ Регистры АЦП.
$7c constant ADMUX
$7a constant ADCSRA
$78 constant ADC

\ Битовые маски для управляющих битов.
$40 constant ADSC

\ Инициализация АЦП: 
\   предделитель 128, канал 0
: adc-init ( -- )
    %10000111 ADCSRA c!
    0 ADMUX c! ;

\ Начать оцифровку.
: adc-start-conv ( -- ) 
    ADSC ADCSRA mset ;

\ Подождать оцифровку.
: adc-wait-conv ( -- )
    begin
    ADSC ADCSRA mtst 0= until ;

\ Измерить напряжение на 0 канале.
: adc-measure ( -- result)
    adc-start-conv
    adc-wait-conv
    ADC @ ;
