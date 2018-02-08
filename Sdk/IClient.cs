//using System;
//using GPWebpayNet.Sdk.Models;
//
//namespace Sdk
//{
//    public interface IClient
//    {
////
////  private string privateKey;
////
////  private string privateKeyPassword;
////
////  private string webPayUrl;
////
////  private string merchantNumber;
//
////  public function __construct ($merchantNumber, $privateKey, $privateKeyPassword, $webPayUrl) {
////    $this->merchantNumber = $merchantNumber;
////    $this->privateKey = $privateKey;
////    $this->privateKeyPassword = $privateKeyPassword;
////    $this->webPayUrl = $webPayUrl;
////  }
//  /**
//   * @param PaymentRequest $request
//   * @return string
//   */
//   string CreatePaymentRequestUrl (PaymentRequest request) {
//    // digest request
//    // build request URL based on PaymentRequest
//    // return URL
//  }
//  /**
//   * @param PaymentResponseParams $params
//   * @return PaymentResponse
//   */
//      PaymentResponse VerifyPayment (PaymentResponseParams parameters) {
//    // verify digest
//    // verify PRCODE and SRCODE
//    // if OK return PaymentResponse
//    // if ERROR throw Exception
//  }
//      
//   void ApproveReversal (ApproveReversalRequest request) {
//    // digest request
//    // call SOAP API
//    // verify response DIGEST
//    // verify PRCODE and SRCODE
//    // if OK return ApproveReversalResponse
//    // if ERROR throw Exception
//  }
//      
//      void Deposit (DepositRequest request) {
//    // same flow as approveReversal
//  }
//      
//      void DepositReversal (DepositReversal request) {
//    // same flow as approveReversal
//  }
//      
//      void credit (CreditRequest request) {
//    // same flow as approveReversal
//  }
//      
//      void CreditReversal (CreditReversal request) {
//    // same flow as approveReversal
//  }
//      
//      void OrderClose (OrderCloseRequest request) {
//    // same flow as approveReversal
//      }
//
//      void Delete(DeleteRequest request)
//      {
//        // same flow as approveReversal
//      }
//
//      void QueryOrderState(QueryOrderStateRequest request)
//      {
//        // same flow as approveReversal
//      }
//
//      void BatchClose(BatchCloseRequest request)
//      {
//        // same flow as approveReversal
//      }
//    }
//}