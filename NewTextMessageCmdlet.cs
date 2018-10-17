using System;
using System.Management.Automation;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace PoshTwilio
{
    [Cmdlet(VerbsCommon.New, "TextMessage")]
    [OutputType(typeof(string))]
    public class SendTextMessage : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string AccountSid { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        public string AuthToken { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        public string Message { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        public string[] ToPhoneNumber { get; set; }

        [Parameter(
            ValueFromPipelineByPropertyName = true)]
        public string FromPhoneNumber { get; set; } // = "XXXXXXXXXX";

        // This method gets called once for each cmdlet in the pipeline when the pipeline starts executing
        protected override void BeginProcessing()
        {
            WriteVerbose($"Begin New-TextMessage to {ToPhoneNumber}");
        }

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {
            try
            {
                WriteVerbose("Starting to Initialize the Twilio Client...");
                TwilioClient.Init(AccountSid, AuthToken);
                WriteVerbose("Finished Initializing the Twilio Client...");
            }
            catch (Exception e)
            {
                throw new Exception($"An exception was thrown initializing the TwilioClient. Exception: {e.Message}");
            }

            WriteVerbose("Begin setting up the message...");
            foreach ( var phoneNumber in ToPhoneNumber )
            {
                var message = MessageResource.Create(
                    body: Message,
                    from: new Twilio.Types.PhoneNumber($"+1{FromPhoneNumber}"),
                    to: new Twilio.Types.PhoneNumber($"+1{phoneNumber}")
                );

                WriteVerbose($"Finished setting up the message: {message.Body} to: {message.To}");

                Console.WriteLine(message.Sid);
            }
        }

        // This method will be called once at the end of pipeline execution; if no input is received, this method is not called
        protected override void EndProcessing()
        {
            WriteVerbose($"End New-TextMessage to {ToPhoneNumber}");
        }
    }
}
