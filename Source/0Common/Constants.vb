Module Constants


  'Public Const ACMaxErrorPercent As Double = 0.0025     ' Have to be within 1/4 of a percent
  'Public Const ACMaxErrorAbsoluteMinGrams As Double = 0.1    '   but no closer than 0.1 grams
  'Public Const ACMaxErrorAbsoluteMaxGrams As Double = 100    '   and at least within 100 grams

  'Public Const ACPoundsToGrams As Double = 453.5924
  'Public Const ACPoundsToKilograms As Double = 0.4536
  'Public Const ACKilogramsToPounds As Double = 2.2046
  'Public Const ACDateFormat As String = "MMMM d, yyyy  hh:mm:ss tt"

  'Public Const comEvSend As Short = 1    'There are fewer than SThreshold number of characters in the transmit buffer. 
  'Public Const comEvReceive As Short = 2 'Received RThreshold number of characters. This event is generated continuously until you use the Input property to remove the data from the receive buffer. 
  'Public Const comEvCTS As Short = 3     'Change in Clear To Send line. 
  'Public Const comEvDSR As Short = 4     'Change in Data Set Ready line. This event is only fired when DSR changes from 1 to 0. 
  'Public Const comEvCD As Short = 5      'Change in Carrier Detect line. 
  'Public Const comEvRing As Short = 6    'Ring detected. Some UARTs (universal asynchronous receiver-transmitters) may not support this event. 
  'Public Const comEvEOF As Short = 7     'End Of File (ASCII character 26) character received. 

  'Public Const comEventBreak As Short = 1001    'A Break signal was received. 
  'Public Const comEventFrame As Short = 1004    'Framing Error. The hardware detected a framing error. 
  'Public Const comEventOverrun As Short = 1006  'Port Overrun. A character was not read from the hardware before the next character arrived and was lost. 
  'Public Const comEventRxOver As Short = 1008   'Receive Buffer Overflow. There is no room in the receive buffer. 
  'Public Const comEventRxParity As Short = 1009 'Parity Error. The hardware detected a parity error. 
  'Public Const comEventTxFull As Short = 1010   'Transmit Buffer Full. The transmit buffer was full while trying to queue a character. 
  'Public Const comEventDCB As Short = 1011      'Unexpected error retrieving Device Control Block (DCB) for the port. 

End Module
