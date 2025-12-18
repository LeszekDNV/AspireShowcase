export interface ApiResponse<T = unknown> {
  success: boolean;
  message?: string;
  data?: T;
  errors?: Record<string, string>;
}

export interface BlobFileInfo {
  name: string;
  size: number;
  lastModified: string;
  contentType: string;
}

export interface Book {
  id: number;
  title: string;
  author: string;
  isbn: string;
  pageCount: number;
  createdAt: string;
}

export interface Message {
  messageId: string;
  subject: string;
  body: string;
  enqueuedTime: string;
  deliveryCount: number;
  properties: Record<string, any>;
}

export interface EmailData {
  to: string;
  subject: string;
}

export interface BlobUploadData {
  fileName: string;
  message: string;
}

export interface MessagesData {
  count: number;
  messages: Message[];
}
