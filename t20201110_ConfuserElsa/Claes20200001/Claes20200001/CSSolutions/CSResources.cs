using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.CSSolutions
{
	public static class CSResources
	{
		#region 予約語リスト

		/// <summary>
		/// ここに含まれる単語は置き換えない。
		/// </summary>
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

Abs
AccessControlType
Action
Add
AddAccessRule
AddFontResourceEx
AddrOfPinnedObject
AggregateException
AllDirectories
Alloc
Allow
Anchor
AnchorStyles
Any
AppDomain
Append
Application
ArgumentException
Array
ASCII
Assembly
AutoScaleDimensions
AutoScaleMode
AutoSize
Begin
BeginInvoke
BltSoftImage
Bottom
Button
ChangeVolumeSoundMem
ChangeWindowMode
CheckBox
Checked
CheckHitKey
CheckSoundMem
CheckState
Clear
Click
ClientSize
ClientToScreen
Close
Collect
Color
Combine
ComboBox
ComboBoxStyle
Comparison
ComponentResourceManager
Compress
CompressionMode
ComputeHash
Concat
Console
Contains
ContainsKey
Controls
Convert
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
DropDownList
DropDownStyle
DuplicateSoundMem
EnableVisualStyles
Encoding
EndsWith
Enqueue
Enum
Enumerable
EnumWindows
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
FormattingEnabled
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
GetFileNameWithoutExtension
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
GetNames
GetNowHiPerformanceCount
GetObject
GetPixelSoftImage
GetRange
GetSoftImageSize
GetString
GetValues
GetWindowText
GraphFilter
GraphicsUnit
GroupBox
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
Items
Join
Keys
LayoutKind
Left
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
MinimumSize
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
NotImplementedException
Now
OK
Open
Padding
Parse
Path
PerformLayout
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
RemoveFontResourceEx
Repeat
Replace
ResumeLayout
Right
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
SelectedIndex
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
SetLoopSamplePosSoundMem
SetLoopStartSamplePosSoundMem
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
SizeGripStyle
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
StreamReader
StreamWriter
StringBuilder
StructLayout
Substring
SuspendLayout
SuspendLayout
SystemEvents
TabIndex
TabStop
Tan
Thread
ThreadException
ThreadExceptionEventArgs
ThreadExceptionEventHandler
ToArray
ToInt64
ToList
ToLower
Top
TopMost
ToString
ToUpper
Trim
UInt16
UInt64
UnhandledException
UnhandledExceptionEventArgs
UnhandledExceptionEventHandler
UnmanagedCode
UseVisualStyleBackColor
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

; デザイナで自動生成されるメソッド
;InitializeComponent

";

		#endregion

		/// <summary>
		/// ここに含まれる単語は置き換えない。
		/// (この単語).(後続の単語).(後続の単語).(後続の単語) ... の「後続の単語」についても置き換えない。
		/// </summary>
		#region 予約語クラス名リスト

		public static string 予約語クラス名リスト = @"

DX

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

		#region クラス用ダミーメンバー

		/// <summary>
		/// 要置き換え : SSS_ to (RANDOM_WORD)_
		/// </summary>
		public static string CLASS_DUMMY_MEMBER = @"

		public static int SSS_Count;

		public int SSS_GetCount()
		{
			return SSS_Count;
		}

		public void SSS_SetCount(int SSS_SetCount_Prm)
		{
			SSS_Count = SSS_SetCount_Prm;
		}

		public void SSS_ResetCount()
		{
			this.SSS_SetCount(0);
		}

		public int SSS_NextCount()
		{
			return SSS_Count++;
		}

		public class SSS_ValueInfo
		{
			public int SSS_ValueInfo_A;
			public int SSS_ValueInfo_B;
			public int SSS_ValueInfo_C;
		}

		public static SSS_ValueInfo SSS_Value;

		public SSS_ValueInfo SSS_GetValue()
		{
			return SSS_Value;
		}

		public void SSS_SetValue(SSS_ValueInfo SSS_SetValue_Prm)
		{
			SSS_Value = SSS_SetValue_Prm;
		}

		public void SSS_Overload_00()
		{
			this.SSS_Overload_01(this.SSS_NextCount());
		}

		public void SSS_Overload_01(int SSS_a)
		{
			this.SSS_Overload_02(SSS_a, this.SSS_NextCount());
		}

		public void SSS_Overload_02(int SSS_a, int SSS_b)
		{
			this.SSS_Overload_03(SSS_a, SSS_b, this.SSS_NextCount());
		}

		public void SSS_Overload_03(int SSS_a, int SSS_b, int SSS_c)
		{
			this.SSS_Overload_04(SSS_a, SSS_b, SSS_c, this.SSS_GetValue().SSS_ValueInfo_A, this.SSS_GetValue().SSS_ValueInfo_B, this.SSS_GetValue().SSS_ValueInfo_C);
		}

		public void SSS_Overload_04(int SSS_a, int SSS_b, int SSS_c, int SSS_a2, int SSS_b2, int SSS_c2)
		{
			this.SSS_SetValue(new SSS_ValueInfo()
			{
				SSS_ValueInfo_A = SSS_a,
				SSS_ValueInfo_B = SSS_b,
				SSS_ValueInfo_C = SSS_c,
			});

			this.SSS_Overload_05(SSS_a2);
			this.SSS_Overload_05(SSS_b2);
			this.SSS_Overload_05(SSS_c2);
		}

		public void SSS_Overload_05(int SSS_v)
		{
			if (SSS_v != this.SSS_GetCount())
				this.SSS_SetCount(SSS_v);
			else
				this.SSS_Overload_01(SSS_v);
		}

";

		#endregion

		#region 構造体用ダミーメンバー

		/// <summary>
		/// 要置き換え : SSS_ to (RANDOM_WORD)_
		/// </summary>
		public static string STRUCT_DUMMY_MEMBER = @"

		public void SSS_Overload_00()
		{
			this.SSS_Overload_01(this.SSS_NextCount());
		}

		public void SSS_Overload_01(int SSS_a)
		{
			this.SSS_Overload_02(SSS_a, this.SSS_NextCount());
		}

		public void SSS_Overload_02(int SSS_a, int SSS_b)
		{
			this.SSS_Overload_03(SSS_a, SSS_b, this.SSS_NextCount());
		}

		public void SSS_Overload_03(int SSS_a, int SSS_b, int SSS_c)
		{
			this.SSS_Overload_04(SSS_a, SSS_b, SSS_c, this.SSS_NextCount());
		}

		public void SSS_Overload_04(int SSS_a, int SSS_b, int SSS_c, int SSS_d)
		{
			this.SSS_AddToCount(SSS_a);
			this.SSS_AddToCount(SSS_b);
			this.SSS_AddToCount(SSS_c);
			this.SSS_AddToCount(SSS_d);
		}

		public static int SSS_Count;

		public int SSS_NextCount()
		{
			return SSS_Count++;
		}

		public void SSS_AddToCount(int SSS_valueForAdd)
		{
			SSS_Count += SSS_valueForAdd;
		}

";

		#endregion
	}
}
