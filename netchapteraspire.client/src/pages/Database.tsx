import './Page.css'
import { useState, useEffect } from 'react'

interface Book {
  id: number
  title: string
  author: string
  isbn: string
  pageCount: number
  createdAt: string
}

interface ApiResponse {
  success: boolean
  message: string
  book?: Book
}

function Database() {
  const [activeTab, setActiveTab] = useState<'add' | 'list'>('add')
  const [books, setBooks] = useState<Book[]>([])
  const [loading, setLoading] = useState(false)
  const [message, setMessage] = useState('')
  const [messageType, setMessageType] = useState<'success' | 'error'>('success')

  // Form state
  const [title, setTitle] = useState('')
  const [author, setAuthor] = useState('')
  const [isbn, setIsbn] = useState('')
  const [pageCount, setPageCount] = useState('')

  const fetchBooks = async () => {
    setLoading(true)
    setMessage('')

    try {
      const response = await fetch('/api/Database/books')
      if (response.ok) {
        const data: Book[] = await response.json()
        setBooks(data)
      } else {
        setMessageType('error')
        setMessage('Error loading books')
      }
    } catch (error) {
      setMessageType('error')
      setMessage('Error connecting to server')
      console.error(error)
    } finally {
      setLoading(false)
    }
  }

  const handleAddBook = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    setMessage('')

    try {
      const response = await fetch('/api/Database/books', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          title,
          author,
          isbn,
          pageCount: parseInt(pageCount),
        }),
      })

      const data: ApiResponse = await response.json()

      if (response.ok && data.success) {
        setMessageType('success')
        setMessage(data.message)
        // Clear form
        setTitle('')
        setAuthor('')
        setIsbn('')
        setPageCount('')
        // Refresh list if on list tab
        if (activeTab === 'list') {
          fetchBooks()
        }
      } else {
        setMessageType('error')
        setMessage(data.message || 'Failed to add book')
      }
    } catch (error) {
      setMessageType('error')
      setMessage('Error connecting to server')
      console.error(error)
    } finally {
      setLoading(false)
    }
  }

  const handleDeleteBook = async (id: number) => {
    if (!confirm('Are you sure you want to delete this book?')) {
      return
    }

    setLoading(true)
    setMessage('')

    try {
      const response = await fetch(`/api/Database/books/${id}`, {
        method: 'DELETE',
      })

      const data: ApiResponse = await response.json()

      if (response.ok && data.success) {
        setMessageType('success')
        setMessage(data.message)
        fetchBooks()
      } else {
        setMessageType('error')
        setMessage(data.message || 'Failed to delete book')
      }
    } catch (error) {
      setMessageType('error')
      setMessage('Error connecting to server')
      console.error(error)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    if (activeTab === 'list') {
      fetchBooks()
    }
  }, [activeTab])

  return (
    <div className="page-container">
      <h1>Database</h1>
      <p className="page-description">
        Manage your book collection with SQL Server and Entity Framework.
      </p>

      {message && (
        <div className={`message ${messageType}`}>
          {message}
        </div>
      )}

      <div className="tabs">
        <button
          className={`tab-button ${activeTab === 'add' ? 'active' : ''}`}
          onClick={() => setActiveTab('add')}
        >
          Add Book
        </button>
        <button
          className={`tab-button ${activeTab === 'list' ? 'active' : ''}`}
          onClick={() => setActiveTab('list')}
        >
          Book List
        </button>
      </div>

      {activeTab === 'add' && (
        <div className="card">
          <h3>Add New Book</h3>
          <form onSubmit={handleAddBook}>
            <div className="form-group">
              <label htmlFor="title">Title:</label>
              <input
                id="title"
                type="text"
                value={title}
                onChange={(e) => setTitle(e.target.value)}
                placeholder="Enter book title"
                required
                disabled={loading}
              />
            </div>

            <div className="form-group">
              <label htmlFor="author">Author:</label>
              <input
                id="author"
                type="text"
                value={author}
                onChange={(e) => setAuthor(e.target.value)}
                placeholder="Enter author name"
                required
                disabled={loading}
              />
            </div>

            <div className="form-group">
              <label htmlFor="isbn">ISBN:</label>
              <input
                id="isbn"
                type="text"
                value={isbn}
                onChange={(e) => setIsbn(e.target.value)}
                placeholder="Enter ISBN"
                required
                disabled={loading}
              />
            </div>

            <div className="form-group">
              <label htmlFor="pageCount">Page Count:</label>
              <input
                id="pageCount"
                type="number"
                value={pageCount}
                onChange={(e) => setPageCount(e.target.value)}
                placeholder="Enter page count"
                required
                min="1"
                disabled={loading}
              />
            </div>

            <button
              type="submit"
              disabled={loading}
              className="send-button"
            >
              {loading ? 'Adding...' : 'Add Book'}
            </button>
          </form>
        </div>
      )}

      {activeTab === 'list' && (
        <div className="card">
          <h3>Book List</h3>
          <button onClick={fetchBooks} disabled={loading} className="refresh-button">
            {loading ? 'Loading...' : 'Refresh List'}
          </button>

          {loading ? (
            <p>Loading books...</p>
          ) : books.length === 0 ? (
            <p>No books found. Add your first book!</p>
          ) : (
            <div className="blob-list">
              <table>
                <thead>
                  <tr>
                    <th>Title</th>
                    <th>Author</th>
                    <th>ISBN</th>
                    <th>Pages</th>
                    <th>Added</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {books.map((book) => (
                    <tr key={book.id}>
                      <td>{book.title}</td>
                      <td>{book.author}</td>
                      <td>{book.isbn}</td>
                      <td>{book.pageCount}</td>
                      <td>{new Date(book.createdAt).toLocaleDateString()}</td>
                      <td>
                        <button
                          onClick={() => handleDeleteBook(book.id)}
                          disabled={loading}
                          className="delete-button"
                        >
                          Delete
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      )}
    </div>
  )
}

export default Database
