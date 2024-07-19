import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import { BrowserRouter } from 'react-router-dom'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { ReactQueryDevtools } from '@tanstack/react-query-devtools'
import { AppProvider } from './context/app.context.tsx'

// QueryCLient được sử dụng để quản lý các truy vấn dữ liệu
const queryClient = new QueryClient({
  // defaultOptions là thuộc tính của queryClient
  // cho phép thiết lập mặt định các vấn
  defaultOptions: {
    queries: {
      //  dùng  refetchOnWindowFocus để tránh refetch dữ liệu lại khi mà chúng ta đòng trang hoặc chuyển tap
      // mặc định sẽ là true, nhưng mình câú hình là false
      // tại sao để tránh refetch làm mới lại dữ liệu hoặc tải dữ liệu không cần thiết

      refetchOnWindowFocus: false,
      // cái retry được dùng để tắt mấy cái toast khi token bị hết hanj
      // nó gọi báo tới 3 lần lận nên là chỉ 1 lần thoi
      retry: 0
    }
  }
})

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    {/* BrowserRouter thằng này được dùng để quản lý app react, được dùng để chuyển trang mà không reload lại trang */}
    <BrowserRouter>
      {/* QueryClientProvider phải bộc lại để quan sát và thực hiện mấy câu truy vấn  */}
      <QueryClientProvider client={queryClient}>
        <AppProvider>
          <App />
        </AppProvider>
        {/*  thằng này giúp theo dỗi và gỡ bỏ các câu truy vấn nếu có lỗi */}
        <ReactQueryDevtools initialIsOpen={false} />
      </QueryClientProvider>
    </BrowserRouter>
  </React.StrictMode>
)
