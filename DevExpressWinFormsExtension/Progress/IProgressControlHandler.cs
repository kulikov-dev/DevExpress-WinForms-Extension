using System.Threading;

namespace DevExpressWinFormsExtension.Progress
{
    /// <summary>
    /// Interface for progress manager
    /// </summary>
    public interface IProgressControlHandler
    {
        /// <summary>
        /// Cancellation token
        /// </summary>
        CancellationToken Token { get; }

        /// <summary>
        /// Message suffix for an iteration progress
        /// </summary>
        string MessageSuffix { get; set; }

        /// <summary>
        /// Increment a progress
        /// </summary>
        /// <param name="value"> Incrementation value </param>
        void Increment(float value);

        /// <summary>
        /// Set progress value
        /// </summary>
        /// <param name="value"> Value </param>
        void SetValue(int value);

        /// <summary>
        /// Set user message
        /// </summary>
        /// <param name="message"> User message </param>
        void SetMessage(string message);

        /// <summary>
        /// Remove a progress control from a parent control
        /// </summary>
        void Drop();

        /// <summary>
        /// Cancel an operation
        /// </summary>
        void Cancel();
    }
}