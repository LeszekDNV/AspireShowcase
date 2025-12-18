import './Page.css'
import { useState } from 'react'
import type { ApiResponse, Message, MessagesData } from '../types/api'

interface SendMessageData {
  queueName: string
  subject: string
}

function ServiceBus() {
  const [activeTab, setActiveTab] = useState<'send' | 'receive'>('send')
  const [loading, setLoading] = useState(false)
  const [message, setMessage] = useState('')
  const [messageType, setMessageType] = useState<'success' | 'error'>('success')

  // Send form state
  const [messageBody, setMessageBody] = useState('')
  const [messageSubject, setMessageSubject] = useState('Demo Message')

  // Received messages
  const [receivedMessages, setReceivedMessages] = useState<Message[]>([])

  const handleSendMessage = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    setMessage('')

    try {
      const response = await fetch('/api/ServiceBus/send', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          message: messageBody,
          subject: messageSubject,
        }),
      })

      const data: ApiResponse<SendMessageData> = await response.json()

      if (data.success) {
        setMessageType('success')
        setMessage(`${data.message}\nQueue: ${data.data?.queueName}\nSubject: ${data.data?.subject}`)
        setMessageBody('')
      } else {
        setMessageType('error')
        setMessage(data.message || 'Failed to send message')
      }
    } catch (error) {
      setMessageType('error')
      setMessage('Error connecting to server')
      console.error(error)
    } finally {
      setLoading(false)
    }
  }

  const handleReceiveMessages = async () => {
    setLoading(true)
    setMessage('')

    try {
      const response = await fetch('/api/ServiceBus/receive?maxMessages=10')
      const data: ApiResponse<MessagesData> = await response.json()

      if (data.success && data.data) {
        setReceivedMessages(data.data.messages || [])
        setMessageType('success')
        setMessage(`Received ${data.data.count} message(s)`)
      } else {
        setMessageType('error')
        setMessage(data.message || 'Failed to receive messages')
      }
    } catch (error) {
      setMessageType('error')
      setMessage('Error connecting to server')
      console.error(error)
    } finally {
      setLoading(false)
    }
  }

  const handlePeekMessages = async () => {
    setLoading(true)
    setMessage('')

    try {
      const response = await fetch('/api/ServiceBus/peek?maxMessages=10')
      const data: ApiResponse<MessagesData> = await response.json()

      if (data.success && data.data) {
        setReceivedMessages(data.data.messages || [])
        setMessageType('success')
        setMessage(`Peeked ${data.data.count} message(s) (messages not removed from queue)`)
      } else {
        setMessageType('error')
        setMessage(data.message || 'Failed to peek messages')
      }
    } catch (error) {
      setMessageType('error')
      setMessage('Error connecting to server')
      console.error(error)
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="page-container">
      <h1>Service Bus</h1>
      <p className="page-description">
        Demonstrate Azure Service Bus capabilities with local emulator.
      </p>

      {message && (
        <div className={`message ${messageType}`}>
          {message.split('\n').map((line, index) => (
            <div key={index}>{line}</div>
          ))}
        </div>
      )}

      <div className="tabs">
        <button
          className={`tab-button ${activeTab === 'send' ? 'active' : ''}`}
          onClick={() => setActiveTab('send')}
        >
          Send Message
        </button>
        <button
          className={`tab-button ${activeTab === 'receive' ? 'active' : ''}`}
          onClick={() => setActiveTab('receive')}
        >
          Receive Messages
        </button>
      </div>

      {activeTab === 'send' && (
        <div className="card">
          <h3>Send Message to Queue</h3>
          <p className="info-text">Send a message to the <strong>demo-queue</strong></p>
          
          <form onSubmit={handleSendMessage}>
            <div className="form-group">
              <label htmlFor="subject">Subject:</label>
              <input
                id="subject"
                type="text"
                value={messageSubject}
                onChange={(e) => setMessageSubject(e.target.value)}
                placeholder="Message subject"
                required
                disabled={loading}
              />
            </div>

            <div className="form-group">
              <label htmlFor="messageBody">Message Body:</label>
              <textarea
                id="messageBody"
                value={messageBody}
                onChange={(e) => setMessageBody(e.target.value)}
                placeholder="Enter your message here..."
                rows={6}
                required
                disabled={loading}
              />
            </div>

            <button
              type="submit"
              disabled={loading}
              className="send-button"
            >
              {loading ? 'Sending...' : 'Send Message'}
            </button>
          </form>
        </div>
      )}

      {activeTab === 'receive' && (
        <div className="card">
          <h3>Receive Messages from Queue</h3>
          <p className="info-text">Retrieve messages from <strong>demo-queue</strong></p>
          
          <div className="button-group">
            <button 
              onClick={handlePeekMessages} 
              disabled={loading} 
              className="refresh-button"
            >
              {loading ? 'Loading...' : 'Peek Messages (Don\'t Remove)'}
            </button>
            <button 
              onClick={handleReceiveMessages} 
              disabled={loading} 
              className="receive-button"
            >
              {loading ? 'Loading...' : 'Receive Messages (Remove from Queue)'}
            </button>
          </div>

          {receivedMessages.length === 0 ? (
            <p className="no-messages">No messages in queue. Send some messages first!</p>
          ) : (
            <div className="messages-container">
              {receivedMessages.map((msg, index) => (
                <div key={index} className="message-card">
                  <div className="message-header">
                    <strong>Subject:</strong> {msg.subject}
                  </div>
                  <div className="message-body">
                    <strong>Body:</strong> {msg.body}
                  </div>
                  <div className="message-meta">
                    <span><strong>Message ID:</strong> {msg.messageId}</span>
                    <span><strong>Enqueued:</strong> {new Date(msg.enqueuedTime).toLocaleString()}</span>
                    <span><strong>Delivery Count:</strong> {msg.deliveryCount}</span>
                  </div>
                  {msg.properties && Object.keys(msg.properties).length > 0 && (
                    <div className="message-properties">
                      <strong>Properties:</strong>
                      <pre>{JSON.stringify(msg.properties, null, 2)}</pre>
                    </div>
                  )}
                </div>
              ))}
            </div>
          )}
        </div>
      )}

      <div className="card">
        <h3>About Azure Service Bus</h3>
        <p>
          This demo uses Azure Service Bus emulator for local development. 
          Messages are sent to and received from a queue called <strong>demo-queue</strong>.
        </p>
        <ul>
          <li><strong>Send Message:</strong> Adds a message to the queue</li>
          <li><strong>Peek Messages:</strong> Views messages without removing them from the queue</li>
          <li><strong>Receive Messages:</strong> Retrieves and removes messages from the queue</li>
        </ul>
      </div>
    </div>
  )
}

export default ServiceBus
