using System;

namespace PROTOCOL
{
	public class GS_COMMUNITY_MESSAGE_ACK
	{
		public enum MessageType
		{
			eLEVELUP,
			eBATTLE_RESULT,
			eMAKE_ITEM,
			eENHANCE_ITEM,
			eLOGIN,
			eLOGOUT,
			eITEMGET,
			eVICTORYBATTLEMATCH,
			eQUESTCOMPLETE,
			eGENERAL_RECRUIT,
			eRESOURCEBATTLERESULT,
			eRESOURCE_NOTIFYATTACKGUILD,
			eSTARTLEAGUE,
			eBABELTOWER_START,
			eITEMSKILL_GET,
			eELEMENTSOL_GET,
			eGUILD_MESSAGE,
			eGENERAL_RECRUIT_LUCKY,
			eITEMGET_FRIEND,
			eITEMGET_FRIENDGUILD
		}

		public enum ReceiveUserType
		{
			ePARTY,
			eFRIEND,
			eGUILD,
			eSPECIAL
		}

		public byte byCunt;

		public byte byMessageType;

		public byte byReceibeUerType;

		public char[] szCharName = new char[21];

		public int i32FaceCharKind;
	}
}
