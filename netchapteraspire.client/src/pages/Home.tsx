import './Page.css'

function Home() {
  return (
    <div className="page-container">
      <h1>Dashboard</h1>
      <p className="page-description">
        Welcome to the NetChapter Aspire Dashboard. Select a service from the navigation menu to get started.
      </p>
      
      <div className="cards-grid">
        <div className="card">
          <h3>Blob Storage</h3>
          <p>Manage Azure Blob Storage resources</p>
        </div>
        <div className="card">
          <h3>Service Bus</h3>
          <p>Manage Azure Service Bus resources</p>
        </div>
        <div className="card">
          <h3>Database</h3>
          <p>Manage database connections</p>
        </div>
        <div className="card">
          <h3>Mailing</h3>
          <p>Manage mailing services</p>
        </div>
      </div>
    </div>
  )
}

export default Home
