import React, { useEffect } from 'react'

// Extend the window object to include subiz

export default function SupportPage() {
  return (
    <div className='bg-white container'>
      <section className='text-center py-12 px-4'>
        <h2 className='text-2xl font-bold'>Nhiệm vụ và giá trị</h2>
        <p className='mt-4 text-gray-700 max-w-2xl mx-auto'>
          Nhiệm vụ của chúng tôi là mang đến cho học sinh những phương pháp học
          tập linh hoạt và tiện lợi, giúp họ dễ dàng tiếp cận với các gia sư
          giỏi nhất theo nhu cầu.
        </p>
        <div className='flex justify-center space-x-8 mt-8 animate-fadeIn'>
          <div className='transition transform hover:scale-110'>
            <h3 className='text-xl font-bold'>85+</h3>
            <p className='text-gray-700'>Gia sư</p>
          </div>
          <div className='transition transform hover:scale-110'>
            <h3 className='text-xl font-bold'>25+</h3>
            <p className='text-gray-700'>Dịch vụ</p>
          </div>
        </div>
      </section>

      <section className='bg-pink-500 text-white py-12 px-4'>
        <h2 className='text-2xl font-bold text-center'>Tầm nhìn</h2>
        <p className='mt-4 text-center max-w-2xl mx-auto'>
          Chúng tôi hướng tới việc cung cấp các dịch vụ gia sư chất lượng cao,
          giúp học sinh phát triển toàn diện và đạt được những thành tích xuất
          sắc trong học tập.
        </p>
      </section>

      <section className='text-center py-12 px-4'>
        <h2 className='text-2xl font-bold'>Các dịch vụ gia sư</h2>
        <div className='grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-8 mt-8'>
          <div className='p-4 shadow-lg rounded-lg bg-blue-100 hover:bg-blue-200 transition-colors'>
            <h3 className='text-xl font-bold'>Toán học</h3>
          </div>
          <div className='p-4 shadow-lg rounded-lg bg-blue-100 hover:bg-blue-200 transition-colors'>
            <h3 className='text-xl font-bold'>Ngữ văn</h3>
          </div>
          <div className='p-4 shadow-lg rounded-lg bg-blue-100 hover:bg-blue-200 transition-colors'>
            <h3 className='text-xl font-bold'>Vật lý</h3>
          </div>
          <div className='p-4 shadow-lg rounded-lg bg-blue-100 hover:bg-blue-200 transition-colors'>
            <h3 className='text-xl font-bold'>Hóa học</h3>
          </div>
        </div>
      </section>

      <section className='bg-gray-100 py-12 px-4'>
        <h2 className='text-2xl font-bold text-center'>Phương pháp học tập</h2>
        <div className='grid grid-cols-1 md:grid-cols-2 gap-8 mt-8 max-w-5xl mx-auto'>
          <div className='p-4 shadow-lg rounded-lg bg-white hover:shadow-xl transition-shadow'>
            <h3 className='text-xl font-bold'>Học trực tuyến</h3>
            <p className='text-gray-700 mt-2'>
              Các buổi học trực tuyến giúp học sinh linh hoạt về thời gian và
              địa điểm học, với sự hỗ trợ tận tình từ gia sư qua các nền tảng
              học trực tuyến.
            </p>
          </div>
          <div className='p-4 shadow-lg rounded-lg bg-white hover:shadow-xl transition-shadow'>
            <h3 className='text-xl font-bold'>Học trực tiếp</h3>
            <p className='text-gray-700 mt-2'>
              Các buổi học trực tiếp tại nhà hoặc địa điểm thuận tiện giúp học
              sinh tương tác trực tiếp với gia sư, nâng cao hiệu quả học tập.
            </p>
          </div>
        </div>
      </section>

      <section className='text-center py-12 px-4'>
        <h2 className='text-2xl font-bold'>
          Cam kết vì sự phát triển của học sinh
        </h2>
        <div className='grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-8 mt-8'>
          <div className='p-4 shadow-lg rounded-lg bg-blue-100 hover:bg-blue-200 transition-colors'>
            <h3 className='text-xl font-bold'>Đặt lịch học</h3>
          </div>
          <div className='p-4 shadow-lg rounded-lg bg-blue-100 hover:bg-blue-200 transition-colors'>
            <h3 className='text-xl font-bold'>Gia sư tận tâm</h3>
          </div>
          <div className='p-4 shadow-lg rounded-lg bg-blue-100 hover:bg-blue-200 transition-colors'>
            <h3 className='text-xl font-bold'>Phát triển toàn diện</h3>
          </div>
          <div className='p-4 shadow-lg rounded-lg bg-blue-100 hover:bg-blue-200 transition-colors'>
            <h3 className='text-xl font-bold'>Nhận tư vấn</h3>
          </div>
        </div>
      </section>

      <section className='bg-pink-400 text-white text-center py-12 px-4'>
        <h2 className='text-2xl font-bold'>Cảm nhận của học sinh</h2>
        <div className='grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-8 mt-8 max-w-5xl mx-auto'>
          <div className='p-4 shadow-lg rounded-lg bg-pink-600 hover:bg-pink-500 transition-colors'>
            <p>
              "Nhờ có gia sư, tôi đã hiểu rõ hơn về các môn học và cải thiện kết
              quả học tập rõ rệt."
            </p>
            <h3 className='mt-4 font-bold'>- Học sinh A</h3>
          </div>
          <div className='p-4 shadow-lg rounded-lg bg-pink-600 hover:bg-pink-500 transition-colors'>
            <p>
              "Gia sư đã giúp tôi tự tin hơn trong việc học và đạt được những
              thành tích cao."
            </p>
            <h3 className='mt-4 font-bold'>- Học sinh B</h3>
          </div>
          <div className='p-4 shadow-lg rounded-lg bg-pink-600 hover:bg-pink-500 transition-colors'>
            <p>
              "Tôi rất hài lòng với dịch vụ gia sư, họ rất nhiệt tình và chuyên
              nghiệp."
            </p>
            <h3 className='mt-4 font-bold'>- Học sinh C</h3>
          </div>
        </div>
      </section>

      <section className='text-center py-12 px-4'>
        <h2 className='text-2xl font-bold'>Liên hệ</h2>
        <div className='mt-8'>
          <p className='text-gray-700'>Email: example@example.com</p>
          <p className='text-gray-700'>Phone: 123-456-7890</p>
          <p className='text-gray-700'>
            Address: 123 Main Street, City, Country
          </p>
        </div>
      </section>
    </div>
  )
}
