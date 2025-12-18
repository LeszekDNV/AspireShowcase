import './Page.css'
import { useState } from 'react'
import type { ApiResponse, EmailData } from '../types/api'

function Mailing() {
  const [loading, setLoading] = useState(false)
  const [message, setMessage] = useState('')
  const [messageType, setMessageType] = useState<'success' | 'error'>('success')
  const [to, setTo] = useState('test@example.com')
  const [subject, setSubject] = useState('Test Email from NetChapter Aspire')
  const [body, setBody] = useState('<h1>Hello!</h1><p>This is a test email.</p>')

  const handleSendMail = async () => {
    setLoading(true)
    setMessage('')

    try {
      const response = await fetch('/api/Mailing/send', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          to,
          subject,
          body,
        }),
      })

      const data: ApiResponse<EmailData> = await response.json()

      if (data.success) {
        setMessageType('success')
        setMessage(`${data.message}\nSent to: ${data.data?.to}`)
      } else {
        setMessageType('error')
        setMessage(data.message || 'Failed to send email')
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
      <h1>Mailing</h1>
      <p className="page-description">
        Send test emails using MailPit SMTP server.
      </p>

      {message && (
        <div className={`message ${messageType}`}>
          {message.split('\n').map((line, index) => (
            <div key={index}>{line}</div>
          ))}
        </div>
      )}

      <div className="card">
        <h3>Send Test Email</h3>
        
        <div className="form-group">
          <label htmlFor="to">To:</label>
          <input
            id="to"
            type="email"
            value={to}
            onChange={(e) => setTo(e.target.value)}
            placeholder="recipient@example.com"
            disabled={loading}
          />
        </div>

        <div className="form-group">
          <label htmlFor="subject">Subject:</label>
          <input
            id="subject"
            type="text"
            value={subject}
            onChange={(e) => setSubject(e.target.value)}
            placeholder="Email subject"
            disabled={loading}
          />
        </div>

        <div className="form-group">
          <label htmlFor="body">Body (HTML):</label>
          <textarea
            id="body"
            value={body}
            onChange={(e) => setBody(e.target.value)}
            placeholder="<h1>Hello!</h1><p>Your message here</p>"
            rows={6}
            disabled={loading}
          />
        </div>

        <button
          onClick={handleSendMail}
          disabled={loading}
          className="send-button"
        >
          {loading ? 'Sending...' : 'Send Mail'}
        </button>
      </div>

      <div className="card">
        <h3>MailPit Web UI</h3>
        <p>
          View sent emails in MailPit web interface at:{' '}
          <a href="http://localhost:1080" target="_blank" rel="noopener noreferrer">
            http://localhost:1080
          </a>
        </p>
      </div>
    </div>
  )
}

export default Mailing
