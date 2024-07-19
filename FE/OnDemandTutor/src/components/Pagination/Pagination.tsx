import { faArrowLeft, faArrowRight } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import classNames from 'classnames'

interface Props {
  totalItems: number
  itemsPerPage: number
  currentPage: number
  onPageChange: (page: number) => void
}

const RANGE = 2

export default function Pagination({
  totalItems,
  itemsPerPage,
  currentPage,
  onPageChange
}: Props) {
  const totalPages = Math.ceil(totalItems / itemsPerPage)

  const renderPagination = () => {
    let dotAfter = false
    let dotBefore = false

    const renderDotBefore = (index: number) => {
      if (!dotBefore) {
        dotBefore = true
        return (
          <span
            key={`dot-before-${index}`}
            className='mx-2 cursor-pointer rounded border bg-white px-3 py-2 shadow-sm'
            onClick={() => onPageChange(index + 1)}
          >
            ...
          </span>
        )
      }
      return null
    }

    const renderDotAfter = (index: number) => {
      if (!dotAfter) {
        dotAfter = true
        return (
          <span
            key={`dot-after-${index}`}
            className='mx-2 cursor-pointer rounded border bg-white px-3 py-2 shadow-sm'
            onClick={() => onPageChange(index + 1)}
          >
            ...
          </span>
        )
      }
      return null
    }

    return Array(totalPages)
      .fill(0)
      .map((_, index) => {
        const pageNumber = index + 1

        if (
          currentPage <= RANGE * 2 + 1 &&
          pageNumber > currentPage + RANGE &&
          pageNumber < totalPages - RANGE + 1
        ) {
          return renderDotAfter(index)
        } else if (
          currentPage > RANGE * 2 + 1 &&
          currentPage < totalPages - RANGE * 2
        ) {
          if (pageNumber < currentPage - RANGE && pageNumber > RANGE) {
            return renderDotBefore(index)
          } else if (
            pageNumber > currentPage + RANGE &&
            pageNumber < totalPages - RANGE + 1
          ) {
            return renderDotAfter(index)
          }
        } else if (
          currentPage >= totalPages - RANGE * 2 &&
          pageNumber > RANGE &&
          pageNumber < currentPage - RANGE
        ) {
          return renderDotBefore(index)
        }

        return (
          <span
            key={`page-${pageNumber}`}
            onClick={() => onPageChange(pageNumber)}
            className={classNames(
              'mx-2 cursor-pointer rounded border bg-white px-3 py-2 shadow-sm',
              {
                'border-cyan-500': pageNumber === currentPage,
                'border-transparent': pageNumber !== currentPage
              }
            )}
          >
            {pageNumber}
          </span>
        )
      })
  }

  return (
    <div className='mt-6 flex flex-wrap justify-center items-center'>
      {currentPage === 1 ? (
        <span className='mx-2 cursor-not-allowed rounded border bg-white/60 px-3 py-2 shadow-sm'>
          <FontAwesomeIcon icon={faArrowLeft} />
        </span>
      ) : (
        <span
          onClick={() => onPageChange(currentPage - 1)}
          className='mx-2 cursor-pointer rounded border bg-white px-3 py-2 shadow-sm'
        >
          <FontAwesomeIcon icon={faArrowLeft} />
        </span>
      )}

      {renderPagination()}

      {currentPage === totalPages ? (
        <span className='mx-2 cursor-not-allowed rounded border bg-white/60 px-3 py-2 shadow-sm'>
          <FontAwesomeIcon icon={faArrowRight} />
        </span>
      ) : (
        <span
          onClick={() => onPageChange(currentPage + 1)}
          className='mx-2 cursor-pointer rounded border bg-white px-3 py-2 shadow-sm'
        >
          <FontAwesomeIcon icon={faArrowRight} />
        </span>
      )}
    </div>
  )
}
