import './Page.css'
import { useState, useEffect } from 'react'
import type { ApiResponse, BlobFileInfo, BlobUploadData } from '../types/api'

function BlobStorage() {
  const [blobs, setBlobs] = useState<BlobFileInfo[]>([])
  const [loading, setLoading] = useState(false)
  const [uploadLoading, setUploadLoading] = useState(false)
  const [message, setMessage] = useState('')
  const [selectedFile, setSelectedFile] = useState<File | null>(null)

  const fetchBlobs = async () => {
    setLoading(true)
    setMessage('')
    try {
      const response = await fetch('/api/BlobStorage/list')
      const data: ApiResponse<BlobFileInfo[]> = await response.json()
      
      if (data.success && data.data) {
        setBlobs(data.data)
      } else {
        setMessage(data.message || 'Error loading blob list')
      }
    } catch (error) {
      setMessage('Error connecting to server')
      console.error(error)
    } finally {
      setLoading(false)
    }
  }

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files.length > 0) {
      setSelectedFile(event.target.files[0])
    }
  }

  const handleUpload = async () => {
    if (!selectedFile) {
      setMessage('Please select a file first')
      return
    }

    setUploadLoading(true)
    setMessage('')
    
    const formData = new FormData()
    formData.append('file', selectedFile)

    try {
      const response = await fetch('/api/BlobStorage/upload', {
        method: 'POST',
        body: formData,
      })

      const result: ApiResponse<BlobUploadData> = await response.json()

      if (result.success && result.data) {
        setMessage(`${result.message}\nFile: ${result.data.fileName}`)
        setSelectedFile(null)
        // Reset file input
        const fileInput = document.getElementById('fileInput') as HTMLInputElement
        if (fileInput) fileInput.value = ''
        // Refresh blob list
        fetchBlobs()
      } else {
        setMessage(result.message || 'Error uploading file')
      }
    } catch (error) {
      setMessage('Error connecting to server')
      console.error(error)
    } finally {
      setUploadLoading(false)
    }
  }

  useEffect(() => {
    fetchBlobs()
  }, [])

  const formatFileSize = (bytes: number) => {
    if (bytes === 0) return '0 Bytes'
    const k = 1024
    const sizes = ['Bytes', 'KB', 'MB', 'GB']
    const i = Math.floor(Math.log(bytes) / Math.log(k))
    return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i]
  }

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleString()
  }

  return (
    <div className="page-container">
      <h1>Blob Storage</h1>
      <p className="page-description">
        Manage your Azure Blob Storage resources here.
      </p>

      {message && (
        <div className={`message ${message.includes('Error') ? 'error' : 'success'}`}>
          {message}
        </div>
      )}

      <div className="card">
        <h3>Send file</h3>
        <div className="upload-section">
          <input
            id="fileInput"
            type="file"
            onChange={handleFileChange}
            disabled={uploadLoading}
          />
          <button 
            onClick={handleUpload} 
            disabled={uploadLoading || !selectedFile}
            className="upload-button"
          >
            {uploadLoading ? 'Uploading...' : 'Upload File'}
          </button>
        </div>
        {selectedFile && (
          <p className="selected-file">Selected: {selectedFile.name}</p>
        )}
      </div>

      <div className="card">
        <h3>Blobs list</h3>
        <button onClick={fetchBlobs} disabled={loading} className="refresh-button">
          {loading ? 'Loading...' : 'Refresh List'}
        </button>
        
        {loading ? (
          <p>Loading blobs...</p>
        ) : blobs.length === 0 ? (
          <p>No blobs found in container</p>
        ) : (
          <div className="blob-list">
            <table>
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Size</th>
                  <th>Content Type</th>
                  <th>Last Modified</th>
                </tr>
              </thead>
              <tbody>
                {blobs.map((blob, index) => (
                  <tr key={index}>
                    <td>{blob.name}</td>
                    <td>{formatFileSize(blob.size)}</td>
                    <td>{blob.contentType || 'N/A'}</td>
                    <td>{formatDate(blob.lastModified)}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  )
}

export default BlobStorage
