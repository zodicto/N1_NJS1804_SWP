import React from 'react'
import { Link } from 'react-router-dom'
import { path } from '../../constant/path'
export default function Footer() {
  return (
    <footer className='  bg-slate-950 mt-[1rem] rounded-2xl shadow-sm  w-full '>
      <div className='mx-auto max-w-screen-xl space-y-8 px-4 py-16 sm:px-6 lg:space-y-16 lg:px-8'>
        <div className='sm:flex sm:items-center sm:justify-between pr-16'>
          <div className='text-gray-100 mx-10 text-3xl'>OTD-Learning</div>

          <ul className='mt-8 flex gap-6 sm:mt-0'>
            <li>
              <a
                href='#'
                rel='noreferrer'
                target='_blank'
                className='text-gray-50 transition hover:text-blue-600'
              >
                <span className='sr-only'>Facebook</span>

                <svg
                  className='h-6 w-6'
                  fill='currentColor'
                  viewBox='0 0 24 24'
                  aria-hidden='true'
                >
                  <path
                    fillRule='evenodd'
                    d='M22 12c0-5.523-4.477-10-10-10S2 6.477 2 12c0 4.991 3.657 9.128 8.438 9.878v-6.987h-2.54V12h2.54V9.797c0-2.506 1.492-3.89 3.777-3.89 1.094 0 2.238.195 2.238.195v2.46h-1.26c-1.243 0-1.63.771-1.63 1.562V12h2.773l-.443 2.89h-2.33v6.988C18.343 21.128 22 16.991 22 12z'
                    clipRule='evenodd'
                  />
                </svg>
              </a>
            </li>

            <li>
              <a
                href='#'
                rel='noreferrer'
                target='_blank'
                className='text-gray-50 transition hover:text-orange'
              >
                <span className='sr-only'>Instagram</span>

                <svg
                  className='h-6 w-6'
                  fill='currentColor'
                  viewBox='0 0 24 24'
                  aria-hidden='true'
                >
                  <path fillRule='evenodd' clipRule='evenodd' />
                </svg>
              </a>
            </li>

            <li>
              <a
                href='#'
                rel='noreferrer'
                target='_blank'
                className='text-gray-50 transition hover:text-blue-700'
              >
                <span className='sr-only'>Twitter</span>

                <svg
                  className='h-6 w-6'
                  fill='currentColor'
                  viewBox='0 0 24 24'
                  aria-hidden='true'
                >
                  <path d='M8.29 20.251c7.547 0 11.675-6.253 11.675-11.675 0-.178 0-.355-.012-.53A8.348 8.348 0 0022 5.92a8.19 8.19 0 01-2.357.646 4.118 4.118 0 001.804-2.27 8.224 8.224 0 01-2.605.996 4.107 4.107 0 00-6.993 3.743 11.65 11.65 0 01-8.457-4.287 4.106 4.106 0 001.27 5.477A4.072 4.072 0 012.8 9.713v.052a4.105 4.105 0 003.292 4.022 4.095 4.095 0 01-1.853.07 4.108 4.108 0 003.834 2.85A8.233 8.233 0 012 18.407a11.616 11.616 0 006.29 1.84' />
                </svg>
              </a>
            </li>

            <li>
              <a
                href='#'
                rel='noreferrer'
                target='_blank'
                className='text-gray-50 transition hover:opacity-75'
              >
                <span className='sr-only'>GitHub</span>

                <svg
                  className='h-6 w-6'
                  fill='currentColor'
                  viewBox='0 0 24 24'
                  aria-hidden='true'
                >
                  <path
                    fillRule='evenodd'
                    d='M12 2C6.477 2 2 6.484 2 12.017c0 4.425 2.865 8.18 6.839 9.504.5.092.682-.217.682-.483 0-.237-.008-.868-.013-1.703-2.782.605-3.369-1.343-3.369-1.343-.454-1.158-1.11-1.466-1.11-1.466-.908-.62.069-.608.069-.608 1.003.07 1.531 1.032 1.531 1.032.892 1.53 2.341 1.088 2.91.832.092-.647.35-1.088.636-1.338-2.22-.253-4.555-1.113-4.555-4.951 0-1.093.39-1.988 1.029-2.688-.103-.253-.446-1.272.098-2.65 0 0 .84-.27 2.75 1.026A9.564 9.564 0 0112 6.844c.85.004 1.705.115 2.504.337 1.909-1.296 2.747-1.027 2.747-1.027.546 1.379.202 2.398.1 2.651.64.7 1.028 1.595 1.028 2.688 0 3.848-2.339 4.695-4.566 4.943.359.309.678.92.678 1.855 0 1.338-.012 2.419-.012 2.747 0 .268.18.58.688.482A10.019 10.019 0 0022 12.017C22 6.484 17.522 2 12 2z'
                    clipRule='evenodd'
                  />
                </svg>
              </a>
            </li>
          </ul>
        </div>

        <div className='grid grid-cols-1 gap-8 border-t border-gray-100 pt-8 sm:grid-cols-2 lg:grid-cols-4 lg:pt-16 pl-28 text-left'>
          <div>
            <p className='font-bold text-green-100'>Các dịch vụ</p>

            <ul className='mt-3 space-y-1 text-sm'>
              <li>
                <Link
                  to={'#'}
                  className='text-gray-400 transition hover:text-pink-500 opacity-90 '
                >
                  {' '}
                  Dạy kèm 1 1{' '}
                </Link>
              </li>
              <li>
                <Link
                  to={'#'}
                  className='text-gray-400 transition hover:text-pink-500 opacity-90 '
                >
                  {' '}
                  Tìm gia sư theo lịch{' '}
                </Link>
              </li>
            </ul>
          </div>
          <div>
            <p className='font-bold text-green-100'>Gia Sư</p>

            <ul className='mt-3 space-y-1 text-sm'>
              <li>
                <Link
                  to={path.registerAsTutor}
                  className='text-gray-400 transition hover:text-pink-500 opacity-90 '
                >
                  Trở Thành Gia Sư
                </Link>
              </li>
              <li>
                <Link
                  to={'#'}
                  className='text-gray-400 transition hover:text-pink-500 opacity-90 '
                >
                  Tìm Gia Sư
                </Link>
              </li>
            </ul>
          </div>
          <div>
            <p className='font-bold text-green-100'>Hệ Thống</p>

            <ul className='mt-3 space-y-1 text-sm'>
              <li>
                <Link
                  to={'/'}
                  className='text-gray-400 transition hover:text-pink-500 opacity-90 '
                >
                  {' '}
                  Trang chủ{' '}
                </Link>
              </li>
              <li>
                <Link
                  to={'#'}
                  className='text-gray-400 transition hover:text-pink-500 opacity-90 '
                >
                  {' '}
                  Thông Tin{' '}
                </Link>
              </li>
            </ul>
          </div>
          <div>
            <p className='font-bold text-green-100'>Chính sách</p>

            <ul className='mt-3 space-y-1 text-sm'>
              <li>
                <Link
                  to={'#'}
                  className='text-gray-400 transition hover:text-pink-500 opacity-90 '
                >
                  {' '}
                  Khả năng của chúng tôi{' '}
                </Link>
              </li>
              <li>
                <Link
                  to={'#'}
                  className='text-gray-400 transition hover:text-pink-500 opacity-90 '
                >
                  {' '}
                  Chính sách hoàn tiền{' '}
                </Link>
              </li>
              <li>
                <Link
                  to={'#'}
                  className='text-gray-400 transition hover:text-pink-500 opacity-90 '
                >
                  {' '}
                  Chính sách giảng dạy{' '}
                </Link>
              </li>
              <li>
                <Link
                  to={'#'}
                  className='text-gray-400 transition hover:text-pink-500 opacity-90 '
                >
                  {' '}
                  Liên hệ với chúng tôi{' '}
                </Link>
              </li>
            </ul>
          </div>
        </div>

        <p className='text-xs text-gray-500'>
          &copy; 2024. OTD Learning. All rights reserved.
        </p>
      </div>
    </footer>
  )
}
