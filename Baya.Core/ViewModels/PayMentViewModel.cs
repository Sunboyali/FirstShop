using System;
using System.Collections.Generic;
using System.Text;

namespace Baya.Core.ViewModels
{

    public class PaymentTotalCard
    {
        public long TotalPrice { get; set; }
    }
    public class RequestPaymentResult
    {
        public int ResCode { get; set; }

        public string Description { get; set; }

        public string Token { get; set; }
    }

    public class CallbackRequestPayment
    {
        public string PrimaryAccNo { get; set; }

        public string HashedCardNo { get; set; }

        public long OrderId { get; set; }

        public string SwitchResCode { get; set; }

        public string ResCode { get; set; }

        public string Token { get; set; }
    }
    public class VerifyResultData
    {
        public int ResCode { get; set; }
        public string Decription { get; set; }
        public string Amount { get; set; }
        public string RetrivalRefNo { get; set; }
        public string SystemTraceNo { get; set; }
        public string OrderId { get; set; }
    }

}
