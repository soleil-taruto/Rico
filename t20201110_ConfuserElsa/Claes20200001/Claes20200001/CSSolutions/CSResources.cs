using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.CSSolutions
{
	public static class CSResources
	{
		#region 予約語リスト

		public static string 予約語リスト = @"

; ====
; C#の予約語
; ====

; --https://ufcpp.net/study/csharp/ap_reserved.html

; キーワード
abstract
as
async
await
base
bool
break
byte
case
catch
char
checked
class
const
continue
decimal
default
delegate
do
double
else
enum
event
explicit
extern
false
finally
fixed
float
for
foreach
goto
if
implicit
in
int
interface
internal
is
lock
long
namespace
new
null
object
operator
out
override
params
private
protected
public
readonly
ref
return
sbyte
sealed
short
sizeof
stackalloc
static
string
struct
switch
this
throw
true
try
typeof
uint
ulong
unchecked
unsafe
ushort
using
virtual
volatile
void
while

; 文脈キーワード
add
dynamic
get
partial
remove
set
value
var
where
yield
when


; ====
; using 名前空間
; ====

AccessControl
Charlotte
Collections
ComponentModel
Compression
Cryptography
Data
Diagnostics
Drawing
DxLibDLL
Forms
Generic
InteropServices
IO
Linq
Microsoft
Permissions
Principal
Reflection
Runtime
Security
System
Text
Threading
Win32
Windows


; ====
; クラス名 / 型名 / メンバー名
; ====

AccessControlType
Action
Add
AddAccessRule
AddrOfPinnedObject
AggregateException
AllDirectories
Alloc
Allow
Any
AppDomain
Append
Application
ArgumentException
Array
Assembly
AutoScaleDimensions
AutoScaleMode
Begin
BeginInvoke
BltSoftImage
ChangeVolumeSoundMem
ChangeWindowMode
CheckHitKey
CheckSoundMem
Clear
ClientSize
Close
Collect
Color
Combine
Comparison
ComponentResourceManager
Compress
CompressionMode
ComputeHash
Concat
Console
Contains
ContainsKey
Copy
CopyTo
Cos
Count
Create
CreateDirectory
CreateFontToHandle
CreateGraphFromSoftImage
Current
CurrentDomain
DateTime
Decompress
Delete
DeleteFontToHandle
DeleteGraph
DeleteSoftImage
DeleteSoundMem
Dequeue
DerivationGraph
Dictionary
Directory
Dispose
DllImport
DrawBox
DrawExtendGraph
DrawExtendGraphF
DrawGraph
DrawGraphF
DrawModiGraph
DrawModiGraphF
DrawPixelSoftImage
DrawString
DrawStringToHandle
DuplicateSoundMem
DX
DX_BLENDMODE_ADD
DX_BLENDMODE_ALPHA
DX_BLENDMODE_INVSRC
DX_BLENDMODE_NOBLEND
DX_CHANGESCREEN_OK
DX_DRAWMODE_ANISOTROPIC
DX_DRAWMODE_NEAREST
DX_FONTTYPE_ANTIALIASING_8X8
DX_FONTTYPE_NORMAL
DX_GRAPH_FILTER_GAUSS
DX_PLAYTYPE_BACK
DX_PLAYTYPE_LOOP
DX_SCREEN_BACK
DxLib_End
DxLib_Init
EnableVisualStyles
Encoding
EndsWith
Enqueue
Enumerable
Environment
Equals
Error
EventArgs
EventHandler
EventHandler
Exception
ExceptionObject
Exists
Exit
File
FileAccess
FileMode
FileStream
FillRectGraph
First
FirstOrDefault
Flags
Font
FontStyle
Form
Format
FormClosed
FormClosedEventArgs
FormClosedEventHandler
FormClosing
FormClosingEventArgs
FormClosingEventHandler
FormStartPosition
Free
FromArgb
FullControl
Func
GC
GCHandle
GCHandleType
GetActiveFlag
GetByteCount
GetBytes
GetColor
GetCommandLineArgs
GetCurrentProcess
GetDefaultState
GetDirectoryName
GetDrawStringWidth
GetDrawStringWidthToHandle
GetEncoding
GetEntryAssembly
GetEnumerator
GetEnvironmentVariable
GetFileName
GetFiles
GetFullPath
GetGraphSize
GetHashCode
GetHitKeyStateAll
GetJoypadInputState
GetJoypadNum
GetMouseDispFlag
GetMouseInput
GetMousePoint
GetMouseWheelRotVol
GetNowHiPerformanceCount
GetObject
GetPixelSoftImage
GetRange
GetSoftImageSize
GetString
GraphFilter
GraphicsUnit
Guid
GZipStream
Handle
HashSet
Icon
IContainer
Id
IDisposable
IECompString
IEnumerable
IEnumerator
IEqualityComparer
IndexOf
Insert
IntPtr
IsNaN
IsNullOrEmpty
Join
KEY_INPUT_A
KEY_INPUT_BACK
KEY_INPUT_C
KEY_INPUT_D
KEY_INPUT_DOWN
KEY_INPUT_ESCAPE
KEY_INPUT_F
KEY_INPUT_LCONTROL
KEY_INPUT_LEFT
KEY_INPUT_RETURN
KEY_INPUT_RETURN
KEY_INPUT_RIGHT
KEY_INPUT_S
KEY_INPUT_SPACE
KEY_INPUT_SPACE
KEY_INPUT_UP
KEY_INPUT_V
KEY_INPUT_X
KEY_INPUT_Z
Keys
LayoutKind
Length
LinkDemand
List
Load
LoadSoftImageToMem
LoadSoundMemByMemImage
Location
Main
MakeARGB8ColorSoftImage
MakeScreen
Manual
Margin
Math
Max
MaximizeBox
MaxValue
MemoryStream
Message
MessageBox
MessageBoxButtons
MessageBoxIcon
MethodInvoker
Min
MinimizeBox
MOUSE_INPUT_LEFT
MOUSE_INPUT_MIDDLE
MOUSE_INPUT_RIGHT
MoveNext
Msg
Mutex
MutexAccessRule
MutexRights
MutexSecurity
Name
NewGuid
Now
OK
Open
Padding
Parse
Path
PI
Pinned
PlaySoundMem
Point
Position
Predicate
Process
ProcessMessage
Queue
RandomNumberGenerator
Range
Read
ReadAllBytes
ReadAllLines
Regular
ReleaseMutex
RemoveAll
RemoveAt
Replace
ResumeLayout
RNGCryptoServiceProvider
Run
ScreenFlip
SearchOption
SecurityAction
SecurityIdentifier
SecurityPermission
SecurityPermissionFlag
Seek
SeekOrigin
Select
Sequential
SessionEnding
SessionEndingEventArgs
SessionEndingEventHandler
SetAlwaysRunFlag
SetApplicationLogSaveDirectory
SetCompatibleTextRenderingDefault
SetCurrentDirectory
SetDrawBlendMode
SetDrawBright
SetDrawBright
SetDrawMode
SetDrawScreen
SetGraphMode
SetMainWindowText
SetMainWindowText
SetMouseDispFlag
SetMousePoint
SetOutApplicationLogValidFlag
SetUseDirectDrawDeviceIndex
SetWindowIconHandle
SetWindowPosition
SetWindowSizeChangeEnableFlag
SHA512
Show
ShowInTaskbar
Shown
Sin
Size
SizeF
Skip
Sleep
Sort
Split
Sqrt
Start
StartPosition
StartsWith
STAThread
StopSoundMem
Stream
StreamWriter
StringBuilder
StructLayout
Substring
SuspendLayout
SuspendLayout
SystemEvents
Tan
Thread
ThreadException
ThreadExceptionEventArgs
ThreadExceptionEventHandler
ToArray
ToInt64
ToList
ToLower
ToString
ToUpper
Trim
UInt16
UnhandledException
UnhandledExceptionEventArgs
UnhandledExceptionEventHandler
UnmanagedCode
UTF8
Value
Visible
WaitOne
WellKnownSidType
Where
WndProc
WorldSid
WParam
Write
WriteAllBytes
WriteByte
WriteLine
Zero

ClientToScreen
EnumWindows
GetWindowText
AddFontResourceEx
RemoveFontResourceEx

UInt64
GetFileNameWithoutExtension

Enum
GetNames
NotImplementedException
Abs
Repeat
KEY_INPUT_PGUP
KEY_INPUT_PGDN
SetLoopSamplePosSoundMem
SetLoopStartSamplePosSoundMem

GroupBox
ComboBox
CheckBox
Button
Anchor
AnchorStyles
Top
Left
DropDownStyle
ComboBoxStyle
DropDownList
StreamReader
Right
TabIndex
TabStop
FormattingEnabled
Click
Controls
AutoSize
Checked
CheckState
UseVisualStyleBackColor
SizeGripStyle
TopMost
PerformLayout
Bottom
KEY_INPUT_E
KEY_INPUT_LSHIFT
KEY_INPUT_RSHIFT
Items
SelectedIndex
MinimumSize

ASCII
Convert

GetValues

; デザイナで自動生成されるメソッド
; -- 本来名前を変更されるはずのないメソッドなので、ウィルス嫌気を避けるため -- 効果あるかは知らん。念の為
InitializeComponent

";

		#endregion

		#region ランダムな単語リスト

		public static string ランダムな単語リスト = @"

; ====
; 太陽系の天体
; ====

; ---- 恒星 ----

Sun

; ---- 惑星 ----

Mercury
Venus
Earth
Mars
Jupiter
Saturn
Uranus
Neptune

; ---- 準惑星 ----

; 地球と火星の間
Ceres

; 海王星の外側
Pluto
Haumea
Makemake
Eris

; ---- 衛星 ----

; -- Earth
; 1
Moon

; -- Mars
; 1
Phobos
; 2
Deimos

; -- Jupiter
; 1
Metis
; 2
Adrastea
; 3
Amalthea
; 4
Thebe
; 5
					;Io
; 6
Europa
; 7
Ganymede
; 8
Callisto
; 9
Themisto
; 10
Leda
; 11
Himalia
; 12
Ersa
; 13
Pandia
; 14
Elara
; 15
Lysithea
; 16
Dia
; 17
Carpo
; 18
;S/2003 J 12
; 19
Valetudo
; 20
Euporie
; 21
;S/2003 J 18
; 22
Harpalyke
; 23
Hermippe
; 24
;S/2017 J 7
; 25
Euanthe
; 26
Thyone
; 27
;S/2016 J 1
; 28
Mneme
; 29
;S/2017 J 3
; 30
Iocaste
; 31
Praxidike
; 32
Ananke
; 33
;S/2003 J 16
; 34
Thelxinoe
; 35
Orthosie
; 36
Helike
; 37
Eupheme
; 38
;S/2010 J 2
; 39
;S/2017 J 9
; 40
;S/2017 J 6
; 41
;S/2011 J 1
; 42
Kale
; 43
Chaldene
; 44
Taygete
; 45
Herse
; 46
Kallichore
; 47
Kalyke
; 48
;S/2003 J 19
; 49
Pasithee
; 50
;S/2003 J 10
; 51
;S/2003 J 23
; 52
Philophrosyne
; 53
Cyllene
; 54
;S/2010 J 1
; 55
Autonoe
; 56
Megaclite
; 57
Eurydome
; 58
;S/2017 J 5
; 59
;S/2017 J 8
; 60
Pasiphae
; 61
Callirrhoe
; 62
;S/2011 J 2
; 63
;S/2017 J 2
; 64
Isonoe
; 65
Aitne
; 66
Hegemone
; 67
Sponde
; 68
Eukelade
; 69
;S/2003 J 4
; 70
Erinome
; 71
Arche
; 72
Eirene
; 73
;S/2003 J 9
; 74
Carme
; 75
Aoede
; 76
Kore
; 77
Sinope
; 78
;S/2017 J 1
; 79
;S/2003 J 2

; -- Saturn
; 1
;S/2009 S 1
; 2
Pan
; 3
Daphnis
; 4
Atlas
; 5
Prometheus
; 6
Pandora
; 7a
Epimetheus
; 7b
Janus
; 10
Mimas
; 11
Methone
; 12
Anthe
; 13
Pallene
; 14
Enceladus
; 15
Tethys
; 15a
Telesto
; 15b
Calypso
; 18
Dione
; 18a
Helene
; 18b
Polydeuces
; 21
Rhea
; 22
Titan
; 23
Hyperion
; 24
Iapetus
; 25
Kiviuq
; 26
Ijiraq
; 27
Phoebe
; 28
Paaliaq
; 29
Skathi
; 30
;S/2004 S 37
; 31
;S/2007 S 2
; 32
Albiorix
; 33
Bebhionn
; 34
;S/2004 S 29
; 35
Erriapus
; 36
;S/2004 S 31
; 37
Skoll
; 38
Siarnaq
; 39
Tarqeq
; 40
;S/2004 S 13
; 41
Hyrrokkin
; 42
Tarvos
; 43
Mundilfari
; 44
;S/2006 S 1
; 45
Greip
; 46
Jarnsaxa
; 47
Bergelmir
; 48
;S/2004 S 17
; 49
Narvi
; 50
;S/2004 S 20
; 51
Suttungr
; 52
Hati
; 53
;S/2004 S 12
; 54
Farbauti
; 55
;S/2004 S 27
; 56
Bestla
; 57
;S/2007 S 3
; 58
Aegir
; 59
;S/2004 S 7
; 60
;S/2004 S 22
; 61
Thrymr
; 62
;S/2004 S 30
; 63
;S/2004 S 23
; 64
;S/2004 S 25
; 65
;S/2004 S 32
; 66
;S/2006 S 3
; 67
;S/2004 S 38
; 68
;S/2004 S 28
; 69
Kari
; 70
;S/2004 S 35
; 71
Fenrir
; 72
;S/2004 S 21
; 73
;S/2004 S 24
; 74
;S/2004 S 36
; 75
Loge
; 76
Surtur
; 77
;S/2004 S 39
; 78
Ymir
; 79
;S/2004 S 33
; 80
;S/2004 S 34
; 81
Fornjot
; 82
;S/2004 S 26

; -- Uranus
; 1
Cordelia
; 2
Ophelia
; 3
Bianca
; 4
Cressida
; 5
Desdemona
; 6
Juliet
; 7
Portia
; 8
Rosalind
; 9
Cupid
; 10
Belinda
; 11
Perdita
; 12
Puck
; 13
Mab
; 14
Miranda
; 15
Ariel
; 16
Umbriel
; 17
Titania
; 18
Oberon
; 19
Francisco
; 20
Caliban
; 21
Stephano
; 22
Trinculo
; 23
Sycorax
; 24
Margaret
; 25
Prospero
; 26
Setebos
; 27
Ferdinand

; -- Neptune
; 1
Naiad
; 2
Thalassa
; 3
Despina
; 4
Galatea
; 5
Larissa
; 6
Hippocamp
; 7
Proteus
; 8
Triton
; 9
Nereid
; 10
Halimede
; 11
Sao
; 12
Laomedeia
; 13
Psamathe
; 14
Neso

; -- Pluto
; 1
Charon
; 2
Styx
; 3
Nix
; 4
Kerberos
; 5
Hydra

; -- Haumea
; 1
Namaka
; 2
;Hi-iaka

; -- Makemake
; 1
;S/2015 (136472) 1

; -- Eris
; 1
;S/2005 (2003 UB 313) 1

; ====
; 元素
; ====

; 1
Hydrogen
; 2
Helium
; 3
Lithium
; 4
Beryllium
; 5
Boron
; 6
Carbon
; 7
Nitrogen
; 8
Oxygen
; 9
Fluorine
; 10
Neon
; 11
Sodium
; 12
Magnesium
; 13
Aluminium
; 14
Silicon
; 15
Phosphorus
; 16
Sulfur
; 17
Chlorine
; 18
Argon
; 19
Potassium
; 20
Calcium
; 21
Scandium
; 22
Titanium
; 23
Vanadium
; 24
Chromium
; 25
Manganese
; 26
Iron
; 27
Cobalt
; 28
Nickel
; 29
Copper
; 30
Zinc
; 31
Gallium
; 32
Germanium
; 33
Arsenic
; 34
Selenium
; 35
Bromine
; 36
Krypton
; 37
Rubidium
; 38
Strontium
; 39
Yttrium
; 40
Zirconium
; 41
Niobium
; 42
Molybdenum
; 43
Technetium
; 44
Ruthenium
; 45
Rhodium
; 46
Palladium
; 47
Silver
; 48
Cadmium
; 49
Indium
; 50
Tin
; 51
Antimony
; 52
Tellurium
; 53
Iodine
; 54
Xenon
; 55
Caesium
; 56
Barium
; 57
Lanthanum
; 58
Cerium
; 59
Praseodymium
; 60
Neodymium
; 61
Promethium
; 62
Samarium
; 63
Europium
; 64
Gadolinium
; 65
Terbium
; 66
Dysprosium
; 67
Holmium
; 68
Erbium
; 69
Thulium
; 70
Ytterbium
; 71
Lutetium
; 72
Hafnium
; 73
Tantalum
; 74
Tungsten
; 75
Rhenium
; 76
Osmium
; 77
Iridium
; 78
Platinum
; 79
Gold
; 80
Mercury
; 81
Thallium
; 82
Lead
; 83
Bismuth
; 84
Polonium
; 85
Astatine
; 86
Radon
; 87
Francium
; 88
Radium
; 89
Actinium
; 90
Thorium
; 91
Protactinium
; 92
Uranium
; 93
Neptunium
; 94
Plutonium
; 95
Americium
; 96
Curium
; 97
Berkelium
; 98
Californium
; 99
Einsteinium
; 100
Fermium
; 101
Mendelevium
; 102
Nobelium
; 103
Lawrencium
; 104
Rutherfordium
; 105
Dubnium
; 106
Seaborgium
; 107
Bohrium
; 108
Hassium
; 109
Meitnerium
; 110
Darmstadtium
; 111
Roentgenium
; 112
Copernicium
; 113
Nihonium
; 114
Flerovium
; 115
Moscovium
; 116
Livermorium
; 117
Tennessine
; 118
Oganesson

; ====
; プリキュア
; ====

; -- All stars
Echo

; -- 無印
Black
White

; -- Max Heart
Luminous

; -- Splush Star
Bloom
Egret
Bright
Windy

; -- 5
Dream
Rouge
Lemonade
Mint
Aqua

; -- 5 GoGo!
Rose

; -- Fresh
Peach
Berry
Pine
Passion

; -- Heart Catch
Blossom
Marine
Sunshine
Moonlight

Flower
;Fire

; -> HUGtto!
;Ange

; -- Suite
Melody
Rhythm
Beat
Muse

; -- Smile
Happy
Sunny
Peace
March
Beauty

; -- Doki^2
Heart
Diamond
Rosetta
Sword
Ace

Empress
Magician
Priestess

;Sebastian
;Cutie Madam

; -- Happiness Charge
Lovely
Princess
Honey
Fortune

Tender

;Bomber Girls
;Wonderful Net

;Merci
Earl

Nile

;Aloha
Sunset
Wave

Continental
Katyusha
;Southern Cross
Gonna
Pantaloni
Matador

;Shelly ???
Sherry

Mirage
;Unlovely

; -- Princess
Flora
Mermaid
Twinkle
Scarlet

; -- 魔法使い
Miracle
Magical
Felice

; -- Kira^2 A La Mode
Whip
Custard
Gelato
Macaron
Chocolat
Parfait

; -- HUGtto!
Yell
Ange
Etoile
Macherie
Amour

Tomorrow

; -- Star Twinkle
Star
Milky
Soleil
Selene
Cosmo

; -- Healin' Good
Grace
Fontaine
Sparkle
Earth

";

		#endregion

		#region ダミーメンバー

		public static string DUMMY_MEMBER = @"

		public int $$_Count;

		public int $$_GetCount()
		{
			return this.$$_Count;
		}

		public void $$_SetCount(int $$_SetCount_Prm)
		{
			this.$$_Count = $$_SetCount_Prm;
		}

		public void $$_ResetCount()
		{
			this.$$_SetCount(0);
		}

		public int $$_NextCount()
		{
			return this.$$_Count++;
		}

		public class $$_ValueInfo
		{
			public int $$_ValueInfo_A;
			public int $$_ValueInfo_B;
			public int $$_ValueInfo_C;
		}

		public $$_ValueInfo $$_Value;

		public $$_ValueInfo $$_GetValue()
		{
			return this.$$_Value;
		}

		public void $$_SetValue($$_ValueInfo $$_SetValue_Prm)
		{
			this.$$_Value = $$_SetValue_Prm;
		}

		public void $$_Overload_00()
		{
			this.$$_Overload_01(this.$$_NextCount());
		}

		public void $$_Overload_01(int $$_a)
		{
			this.$$_Overload_02($$_a, this.$$_NextCount());
		}

		public void $$_Overload_02(int $$_a, int $$_b)
		{
			this.$$_Overload_03($$_a, $$_b, this.$$_NextCount());
		}

		public void $$_Overload_03(int $$_a, int $$_b, int $$_c)
		{
			this.$$_Overload_04($$_a, $$_b, $$_c, this.$$_GetValue().$$_ValueInfo_A, this.$$_GetValue().$$_ValueInfo_B, this.$$_GetValue().$$_ValueInfo_C);
		}

		public void $$_Overload_04(int $$_a, int $$_b, int $$_c, int $$_a2, int $$_b2, int $$_c2)
		{
			this.$$_SetValue(new $$_ValueInfo()
			{
				$$_ValueInfo_A = $$_a,
				$$_ValueInfo_B = $$_b,
				$$_ValueInfo_C = $$_c,
			});

			this.$$_Overload_05($$_a2);
			this.$$_Overload_05($$_b2);
			this.$$_Overload_05($$_c2);
		}

		public void $$_Overload_05(int $$_v)
		{
			if ($$_v != this.$$_GetCount())
				this.$$_SetCount($$_v);
			else
				this.$$_Overload_01($$_v);
		}

";

		#endregion
	}
}
